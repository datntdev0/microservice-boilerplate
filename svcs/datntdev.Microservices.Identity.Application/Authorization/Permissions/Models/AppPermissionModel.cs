using datntdev.Microservices.Common.Authorization.Permissions;

namespace datntdev.Microservices.Identity.Application.Authorization.Permissions.Models
{
    public class AppPermissionModel(AppPermission id, string name, string description, AppPermissionModel[]? children = null)
    {
        public AppPermission Id { get; set; } = id;

        public string Name { get; set; } = name;

        public string Description { get; set; } = description;

        public AppPermissionModel[] Children { get; set; } = children ?? [];
    }
}
