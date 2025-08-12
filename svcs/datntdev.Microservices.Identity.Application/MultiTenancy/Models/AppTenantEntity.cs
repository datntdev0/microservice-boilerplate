using AutoMapper;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos;

namespace datntdev.Microservices.Identity.Application.MultiTenancy.Models
{
    [AutoMap(typeof(TenantDto), ReverseMap = true)]
    [AutoMap(typeof(TenantListDto), ReverseMap = true)]
    public class AppTenantEntity : FullAuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<AppUserEntity> Users { get; set; } = [];
        public List<AppTenantUserEntity> TenantUsers { get; set; } = [];
    }
}
