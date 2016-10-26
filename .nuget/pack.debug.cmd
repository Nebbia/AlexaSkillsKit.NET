nuget pack ../AlexaSkillsKit.Lib/AlexaSkillsKit.Lib.csproj -Build -properties Configuration=Debug -symbols
nuget pack ../AlexaSkillsKit.Lib.ServiceStack/AlexaSkillsKit.Lib.ServiceStack.csproj -Build  -properties Configuration=Debug -symbols

pause