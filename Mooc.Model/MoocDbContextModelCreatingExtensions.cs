using Mooc.Shared.Entity.Admin;

public static class MoocDbContextModelCreatingExtensions
{
    private const string TablePrefix = "";
    //private const string Schema = "";

    /// <summary>
    /// Admin
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ConfigureAdminManagement(this ModelBuilder modelBuilder)
    {
        //Menu
        modelBuilder.Entity<Menu>(b =>
        {
            b.ToTable(TablePrefix + "Menu");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.HasIndex(x => x.ParentId);
            b.Property(cs => cs.Title).IsRequired().HasMaxLength(MenuEntityConsts.MaxTitleLength);
            b.Property(cs => cs.Permission).HasMaxLength(MenuEntityConsts.MaxPermissionLength);
            b.Property(cs => cs.Mark).HasMaxLength(MenuEntityConsts.MaxMarkLength);
            b.Property(cs => cs.Route).HasMaxLength(MenuEntityConsts.MaxRouteLength);
            b.Property(cs => cs.ComponentPath).HasMaxLength(MenuEntityConsts.MaxComponentPathLength);
            b.Property(cs => cs.MenuType).HasConversion(
               v => v.ToString(),
               v => (MenuType)Enum.Parse(typeof(MenuType), v)
           ).HasMaxLength(MenuEntityConsts.MaxMenuTypeLength);
            b.HasMany(cs => cs.RoleMenus);
            b.HasOne(cs => cs.Parent).WithMany(cs => cs.Children).HasForeignKey(cs => cs.ParentId);
        });

        modelBuilder.Entity<RoleMenu>(b =>
        {
            b.ToTable(TablePrefix + "RoleMenu");
            b.HasKey(x => x.Id);
        });

        //User
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable(TablePrefix + "User");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(cs => cs.UserName).IsRequired().HasMaxLength(UserEntityConsts.MaxUserNameLength);
            b.Property(cs => cs.Password).HasMaxLength(UserEntityConsts.MaxPasswordLength);
            b.Property(cs => cs.Email).HasMaxLength(UserEntityConsts.MaxEmailLength);
            b.Property(cs => cs.Address).HasMaxLength(UserEntityConsts.MaxAddressLength);
            b.Property(cs => cs.Phone).HasMaxLength(UserEntityConsts.MaxPhoneLength);
            b.Property(cs => cs.Avatar).HasMaxLength(UserEntityConsts.MaxAvatarLength);
            b.HasMany(x => x.UserRoles);//.WithOne().HasForeignKey(p => p.UserId);已经再对象里面有 UserId 就不需要用 ，不然会出现多一个同样的外键 UserId1
            b.Property(cs => cs.Gender).HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)
            ).HasMaxLength(UserEntityConsts.MaxGenderLength);
        });

        //Role
        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable(TablePrefix + "Role");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(cs => cs.RoleName).IsRequired().HasMaxLength(RoleEntityConsts.MaxRoleNameLength);
            b.Property(cs => cs.Mrak).HasMaxLength(RoleEntityConsts.MaxRemarkLength);
            b.HasMany(x => x.UserRoles);//.WithOne().HasForeignKey(p => p.RoleId);已经再对象里面有 RoleId 就不需要用 ，不然会出现多一个同样的外键RoleId1
            b.HasMany(x => x.RoleMenus);
        });

        //UserRole
        modelBuilder.Entity<UserRole>(b =>
        {
            b.ToTable(TablePrefix + "UserRole");
        });
    }




 
}
