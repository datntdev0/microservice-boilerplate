namespace datntdev.Microservices.Common.Authorization.Permissions
{
    public enum AppPermission
    {
        None = 0,

        RoleManagement = 2000,
        ViewRole = 2001,
        ModifyRole = 2002,

        UserManagement = 2100,
        ViewUser = 2101,
        ModifyUser = 2102, 
        AssignRole = 2103,

        PermissionManagement = 2200,
        ViewPermission = 2201,
        AssignRolePermission = 2202,
        AssignUserPermission = 2203,
    }
}
