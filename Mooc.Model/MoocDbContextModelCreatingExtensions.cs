using Mooc.Shared.Entity.Admin;

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
        modelBuilder.Entity<MoocUser>(b =>
        {
            b.ToTable(TablePrefix + "MoocUser");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.UserName).IsRequired().HasMaxLength(UserEntityConsts.MaxUserNameLength);
            b.Property(x => x.Password).HasMaxLength(UserEntityConsts.MaxPasswordLength);
            b.Property(x => x.Email).HasMaxLength(UserEntityConsts.MaxEmailLength);
            b.Property(x => x.Age).HasMaxLength(UserEntityConsts.MaxAgeLength);
            b.HasMany(x => x.MoocUserRole);
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
    }
}
