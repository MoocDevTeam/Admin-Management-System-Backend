namespace Mooc.Shared;

public class PermissionConsts
{
    public const string PermissionManagement = "Admin";
    public class User
    {
        public const string Default = PermissionManagement + ".User";
        public const string Add = Default + ".Add";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public class Role
    {
        public const string Default = PermissionManagement + ".Role";
        public const string Add = Default + ".Add";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }


}
