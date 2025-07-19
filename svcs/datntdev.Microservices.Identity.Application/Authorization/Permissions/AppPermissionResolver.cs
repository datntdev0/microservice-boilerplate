using datntdev.Microservices.Common.Authorization.Permissions;
using datntdev.Microservices.Identity.Application.Authorization.Permissions.Models;

namespace datntdev.Microservices.Identity.Application.Authorization.Permissions
{
    public static class AppPermissionResolver
    {
        private static readonly AppPermissionModel[] _appPermissions =
        [
            // Role Management Permissions
            new AppPermissionModel(AppPermission.RoleManagement, "Role Management", "Manage application roles.", [
                new AppPermissionModel(AppPermission.ViewRole, "View Role", "View role details."),
                new AppPermissionModel(AppPermission.ModifyRole, "Modify Role", "Update role information (not permissions)."),
            ]),

            // User Management Permissions
            new AppPermissionModel(AppPermission.UserManagement, "User Management", "Manage application users.", [
                new AppPermissionModel(AppPermission.ViewUser, "View User", "View user details."),
                new AppPermissionModel(AppPermission.ModifyUser, "Modify User", "Update user information (not roles or permissions)."),
                new AppPermissionModel(AppPermission.AssignRole, "Assign Role", "Assign roles to users."),
            ]),

            // Permission Management Permissions
            new AppPermissionModel(AppPermission.PermissionManagement, "Permission Management", "Manage permissions in the system.", [
                new AppPermissionModel(AppPermission.ViewPermission, "View Permission", "View permission details."),
                new AppPermissionModel(AppPermission.AssignRolePermission, "Assign Role Permission", "Assign permissions to roles."),
                new AppPermissionModel(AppPermission.AssignUserPermission, "Assign User Permission", "Assign permissions to users."),
            ]),
        ];

        public static AppPermissionModel[] RootPermissions { get; } = _appPermissions;

        internal static AppPermission[] GetAppPermissionEnums(AppPermissionModel[] permissions)
        {
            // Recursive local function to collect enum values
            static IEnumerable<AppPermission> FlattenEnums(AppPermissionModel perm) =>
                new[] { perm.Id }.Concat(perm.Children?.SelectMany(FlattenEnums) ?? []);

            // Collect all enums from the permissions hierarchy
            return [.. permissions.SelectMany(FlattenEnums).Distinct()];
        }
    }
}
