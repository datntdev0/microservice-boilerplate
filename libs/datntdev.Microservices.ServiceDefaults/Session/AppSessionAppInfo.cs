namespace datntdev.Microservices.ServiceDefaults.Session
{ 
    public class AppSessionAppInfo
    {
        public required string Theme { get; set; }

        public static AppSessionAppInfo Default => new()
        {
            Theme = "light"
        };
    }
}
