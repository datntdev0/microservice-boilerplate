namespace datntdev.Microservices.ServiceDefaults.Session
{
    public class AppSessionTenancyInfo
    {
        public int TenantId { get; set; }

        public static AppSessionTenancyInfo HostTenant => new()
        {
            TenantId = 0
        };
    }
}
