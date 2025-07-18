using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Registars;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.ServiceDefaults.Session
{
    [InjectService(ServiceLifetime.Scoped)]
    public class AppSessionContext
    {
        public AppSessionTenancyInfo? TenantInfo { get; private set; }

        public AppSessionUserInfo? UserInfo { get; private set; }

        public int? TenantId => TenantInfo?.TenantId;

        public bool IsHostTenant => TenantInfo?.TenantId == Constants.Tenancy.HostTenantId;

        public void SetTenantInfo(AppSessionTenancyInfo? tenantInfo)
        {
            TenantInfo = tenantInfo;
        }

        public void SetUserInfo(AppSessionUserInfo? userInfo)
        {
            UserInfo = userInfo;
        }
    }
}
