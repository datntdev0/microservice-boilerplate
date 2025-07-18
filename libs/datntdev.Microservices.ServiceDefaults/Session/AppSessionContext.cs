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

        public void SetTenantInfo(AppSessionTenancyInfo? tenantInfo)
        {
            TenantInfo = tenantInfo;
        }

        public void SetUserInfo(HttpContext httpContext)
        {
            var currentUser = httpContext.User;
            UserInfo = currentUser.Identity?.IsAuthenticated == false ? null :
                new AppSessionUserInfo
                {
                    UserId = long.Parse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    Username = currentUser.FindFirstValue(ClaimTypes.Name)!,
                    EmailAddress = currentUser.FindFirstValue(ClaimTypes.Email)!,
                    FirstName = currentUser.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = currentUser.FindFirstValue(ClaimTypes.Surname)!,
                };
        }
    }
}
