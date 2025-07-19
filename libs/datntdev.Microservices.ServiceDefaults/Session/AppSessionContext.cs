using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Registars;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace datntdev.Microservices.ServiceDefaults.Session
{
    [InjectService(ServiceLifetime.Scoped)]
    public class AppSessionContext
    {
        public AppSessionTenancyInfo? TenantInfo { get; private set; }

        public AppSessionUserInfo? UserInfo { get; private set; }

        public AppSessionAppInfo AppInfo { get; private set; } = AppSessionAppInfo.Default;

        public int? TenantId => TenantInfo?.TenantId;

        public bool IsHostTenant => TenantInfo?.TenantId == Constants.Tenancy.HostTenantId;

        public AppSessionContext SetTenantInfo(AppSessionTenancyInfo? tenantInfo)
        {
            TenantInfo = tenantInfo;
            return this;
        }

        public AppSessionContext SetUserInfo(AppSessionUserInfo? userInfo)
        {
            UserInfo = userInfo;
            return this;
        }
    }
}
