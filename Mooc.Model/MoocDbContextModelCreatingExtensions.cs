using Mooc.Shared.Entity.Admin;
using Mooc.Shared.Enum;

public static class MoocDbContextModelCreatingExtensions
{
    private const string TablePrefix = "";


    /// <summary>
    /// Admin
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ConfigureAdminManagement(this ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable(TablePrefix + "User");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.UserName).IsRequired().HasMaxLength(UserEntityConsts.MaxUserNameLength);
            b.Property(x => x.Password).HasMaxLength(UserEntityConsts.MaxPasswordLength);
            b.Property(x => x.Email).HasMaxLength(UserEntityConsts.MaxEmailLength);
            b.Property(x => x.Age).HasMaxLength(UserEntityConsts.MaxAgeLength);
            b.HasMany(x => x.UserRole);
            b.Property(x => x.Avatar).HasMaxLength(UserEntityConsts.MaxAvatarLength);
            b.Property(x => x.Gender).HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v));
            b.Property(x => x.Access).HasConversion(
                v => v.ToString(),
                v => (Access)Enum.Parse(typeof(Access), v));
            b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            b.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");//for SQLite


        });

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
            //b.HasMany(cs => cs.RoleMenus);
            b.HasOne(cs => cs.Parent).WithMany(cs => cs.Children).HasForeignKey(cs => cs.ParentId);
        });

        //modelBuilder.Entity<RoleMenu>(b =>
        //{
        //    b.ToTable(TablePrefix + "RoleMenu");
        //    b.HasKey(x => x.Id);
        //});
    }
    
}
