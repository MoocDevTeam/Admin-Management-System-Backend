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
        ConfigureMenu(modelBuilder);
        ConfigureRole(modelBuilder);
        ConfigureRoleMenu(modelBuilder);
        ConfigureUserRole(modelBuilder);
        ConfigureCarousel(modelBuilder);
        ConfigureComment(modelBuilder);
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
            b.HasMany(x => x.UserRoles);
            b.Property(x => x.Avatar).HasMaxLength(UserEntityConsts.MaxAvatarLength);
            b.Property(x => x.Gender).HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v));
            b.Property(x => x.Access).HasConversion(
                v => v.ToString(),
                v => (Access)Enum.Parse(typeof(Access), v));
            b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            b.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");//for SQLite
        });
    }

    private static void ConfigureMenu(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(b =>
        {
            b.ToTable(TablePrefix + "Menu");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.HasIndex(x => x.ParentId);
            b.Property(cs => cs.Title).IsRequired().HasMaxLength(MenuEntityConsts.MaxTitleLength);
            b.Property(cs => cs.Permission).HasMaxLength(MenuEntityConsts.MaxPermissionLength);
            b.Property(cs => cs.Route).HasMaxLength(MenuEntityConsts.MaxRouteLength);
            b.Property(cs => cs.ComponentPath).HasMaxLength(MenuEntityConsts.MaxComponentPathLength);
            b.Property(cs => cs.MenuType).HasConversion(
               v => v.ToString(),
               v => (MenuType)Enum.Parse(typeof(MenuType), v)
           ).HasMaxLength(MenuEntityConsts.MaxMenuTypeLength);
            b.HasMany(cs => cs.RoleMenus);
            b.HasOne(cs => cs.Parent).WithMany(cs => cs.Children).HasForeignKey(cs => cs.ParentId);
        });
    }
    private static void ConfigureRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable(TablePrefix + "Role");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.RoleName).IsRequired().HasMaxLength(RoleEntityConsts.MaxRoleNameLength);
            b.Property(x => x.Description).IsRequired().HasMaxLength(RoleEntityConsts.MaxDescriptionLength);
            b.HasMany(cs => cs.RoleMenus);
            b.HasMany(x => x.UserRoles);
        });
    }

    private static void ConfigureRoleMenu(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleMenu>(b =>
        {
            b.ToTable(TablePrefix + "RoleMenu");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.HasKey(rm => new { rm.RoleId, rm.MenuId });
            b.HasOne(rm => rm.Role)
              .WithMany(r => r.RoleMenus)
              .HasForeignKey(rm => rm.RoleId);
            b.HasOne(rm => rm.Menu)
             .WithMany(m => m.RoleMenus)
             .HasForeignKey(rm => rm.MenuId);        
        });
    }

    private static void ConfigureUserRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>(b =>
        {
            b.ToTable(TablePrefix + "UserRole");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.HasKey(ur => new { ur.UserId, ur.RoleId });
            b.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId);
            b.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId);          
        });


    }

    private static void ConfigureCarousel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carousel>(b =>
        {
            b.ToTable("Carousel");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(CarouselEntityConsts.MaxTitleLength);
            b.Property(x => x.ImageUrl).IsRequired();
            b.Property(x => x.RedirectUrl).HasMaxLength(CarouselEntityConsts.MaxRedirectUrlLength);
            b.Property(x => x.IsActive).IsRequired().HasDefaultValue(CarouselEntityConsts.DefaultIsActive);
            b.Property(x => x.UpdatedAt).IsRequired();
            b.Property(x => x.StartDate).IsRequired();
            b.Property(x => x.EndDate).IsRequired();
            b.Property(x => x.Position).IsRequired();
            b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserId);
            b.HasOne<User>().WithMany().HasForeignKey(x => x.UpdatedByUserId);
        });
    }

    private static void ConfigureComment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(b =>
        {
            b.ToTable("Comment");
            b.HasKey(x => x.Id);
            b.Property(x => x.Content).IsRequired().HasMaxLength(CommentEntityConsts.MaxContentLength);
            b.Property(x => x.IsActive).IsRequired().HasDefaultValue(CommentEntityConsts.DefaultIsActive);
            b.Property(x => x.IsFlagged).IsRequired().HasDefaultValue(CommentEntityConsts.DefaultIsFlagged);
            b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserId);
            b.HasOne<Comment>().WithMany().HasForeignKey(x => x.ParentCommentId);
        });
    }
}
