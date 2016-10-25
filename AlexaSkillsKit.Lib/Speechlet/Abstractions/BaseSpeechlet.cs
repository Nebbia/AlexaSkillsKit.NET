using System;
using System.Linq;
using System.Text;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet.Abstractions
{
    public abstract class BaseSpeechlet<T, TResponse, TResponseFactory, TRequest> : ISpeechlet where T : IAbstractHttpRequest
        where TResponse : new()
        where TResponseFactory : IHttpResponseFactory<TResponse>, new()
    {
        /// <summary>
        ///     Opportunity to set policy for handling requests with invalid signatures and/or timestamps
        /// </summary>
        /// <returns>true if request processing should continue, otherwise false</returns>
        public virtual bool OnRequestValidation(
            SpeechletRequestValidationResult result, DateTime referenceTimeUtc, SpeechletRequestEnvelope requestEnvelope)
        {
            return result == SpeechletRequestValidationResult.OK;
        }


        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session);
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session);
        public abstract void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);
        public abstract void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);

        /// <summary>
        ///     Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public virtual TResponse GetResponse(T httpRequest)
        {
            var validationResult = SpeechletRequestValidationResult.OK;
            var now = DateTime.UtcNow; // reference time for this request

            string chainUrl = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER) ||
                string.IsNullOrEmpty(
                    chainUrl = httpRequest.Headers.GetValue(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).First()))
                validationResult = validationResult | SpeechletRequestValidationResult.NoCertHeader;

            string signature = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER) ||
                string.IsNullOrEmpty(signature = httpRequest.Headers.GetValue(Sdk.SIGNATURE_REQUEST_HEADER).First()))
                validationResult = validationResult | SpeechletRequestValidationResult.NoSignatureHeader;

            var alexaBytes = AsyncHelpers.RunSync(() => httpRequest.Content.ReadAsByteArrayAsync());
            var alexaContent = Encoding.UTF8.GetString(alexaBytes);
            //Debug.WriteLine(httpRequest.ToLogString()); //todo

            // attempt to verify signature only if we were able to locate certificate and signature headers
            if (validationResult == SpeechletRequestValidationResult.OK)
                if (!SpeechletRequestSignatureVerifier.VerifyRequestSignature(alexaBytes, signature, chainUrl))
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidSignature;

            SpeechletRequestEnvelope alexaRequest = null;
            try
            {
                alexaRequest = SpeechletRequestEnvelope.FromJson(alexaContent);
            }
            catch (JsonReaderException)
            {
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }
            catch (InvalidCastException)
            {
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }

            // attempt to verify timestamp only if we were able to parse request body
            if (alexaRequest != null)
                if (!SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(alexaRequest, now))
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidTimestamp;

            if ((alexaRequest == null) || !OnRequestValidation(validationResult, now, alexaRequest))
            {
                var responseFactory = new TResponseFactory();
                return responseFactory.BadRequest(validationResult.ToString());
            }

            var alexaResponse = DoProcessRequest(alexaRequest);

            if (alexaResponse == null)
            {
                var responseFactory = new TResponseFactory();
                return responseFactory.InternalServerError();
            }
            else
            {
                var responseFactory = new TResponseFactory();
                return responseFactory.Ok(alexaResponse);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="requestContent"></param>
        /// <returns></returns>
        public virtual string ProcessRequest(string requestContent)
        {
            var requestEnvelope = SpeechletRequestEnvelope.FromJson(requestContent);
            return DoProcessRequest(requestEnvelope);
        }


        /// <summary>
        /// </summary>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        public virtual string ProcessRequest(JObject requestJson)
        {
            var requestEnvelope = SpeechletRequestEnvelope.FromJson(requestJson);
            return DoProcessRequest(requestEnvelope);
        }


        /// <summary>
        /// </summary>
        /// <param name="requestEnvelope"></param>
        /// <returns></returns>
        private string DoProcessRequest(SpeechletRequestEnvelope requestEnvelope)
        {
            var session = requestEnvelope.Session;
            SpeechletResponse response = null;

            // process launch request
            if (requestEnvelope.Request is LaunchRequest)
            {
                var request = requestEnvelope.Request as LaunchRequest;
                if (requestEnvelope.Session.IsNew)
                    OnSessionStarted(
                        new SessionStartedRequest(request.RequestId, request.Timestamp), session);
                response = OnLaunch(request, session);
            }

            // process intent request
            else if (requestEnvelope.Request is IntentRequest)
            {
                var request = requestEnvelope.Request as IntentRequest;

                // Do session management prior to calling OnSessionStarted and OnIntentAsync 
                // to allow dev to change session values if behavior is not desired
                DoSessionManagement(request, session);

                if (requestEnvelope.Session.IsNew)
                    OnSessionStarted(
                        new SessionStartedRequest(request.RequestId, request.Timestamp), session);
                response = OnIntent(request, session);
            }

            // process session ended request
            else if (requestEnvelope.Request is SessionEndedRequest)
            {
                var request = requestEnvelope.Request as SessionEndedRequest;
                OnSessionEnded(request, session);
            }

            var responseEnvelope = new SpeechletResponseEnvelope
            {
                Version = requestEnvelope.Version,
                Response = response,
                SessionAttributes = requestEnvelope.Session.Attributes
            };
            return responseEnvelope.ToJson();
        }


        /// <summary>
        /// </summary>
        private void DoSessionManagement(IntentRequest request, Session session)
        {
            if (session.IsNew)
            {
                session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
            }
            else
            {
                // if the session was started as a result of a launch request 
                // a first intent isn't yet set, so set it to the current intent
                if (!session.Attributes.ContainsKey(Session.INTENT_SEQUENCE))
                    session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
                else
                    session.Attributes[Session.INTENT_SEQUENCE] += Session.SEPARATOR + request.Intent.Name;
            }

            // Auto-session management: copy all slot values from current intent into session
            foreach (var slot in request.Intent.Slots.Values)
                session.Attributes[slot.Name] = slot.Value;
        }
    }
}