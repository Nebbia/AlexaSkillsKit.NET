using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ServiceStack;

namespace AlexaSkillsKit.Sample.ServiceStack
{
    public class Global : System.Web.HttpApplication
    {


        public class AppHost : AppHostBase
        {
            //Tell ServiceStack the name of your application and where to find your services
            public AppHost() : base("Hello Web Services", typeof(SkillService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());

                PreRequestFilters.Add((httpReq, httpRes) => {
                    httpReq.UseBufferedStream = true;
                });
            }
        }

        //Initialize your application singleton
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}