namespace WebApiPractice.Authority
{
    public static class AppRepository
    {
       private static List<Application> _applications=new List<Application>()
       {
           new Application()
           {
                ApplicationId = 1,
                 ApplicationName = "WebApp",
                  ClientId="778838B7-DCF3-4CA6-9B21-00980664898F",
                   Secret="6BAAD346-CE07-4F2F-817E-E18626119B1F",
                   Scopes="Read,Write,Delete"
           }
       };
        public static bool Authenticate(string? clientid,string? secret)
        {
            return _applications.Any(x=>x.ClientId == clientid && x.Secret == secret);
        }
        public static Application? GetApplicationByClientId(string? clientid)
        {
            return _applications.FirstOrDefault(x=>x.ClientId == clientid); 
        }
    }
}
