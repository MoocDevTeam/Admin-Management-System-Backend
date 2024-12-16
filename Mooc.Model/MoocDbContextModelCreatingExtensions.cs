using Mooc.Shared.Entity.Admin;
using Mooc.Shared.Entity.Course;

public static class MoocDbContextModelCreatingExtensions
{
    private const string TablePrefix = "";


    /// <summary>
    /// Admin
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ConfigureAdminManagement(this ModelBuilder modelBuilder)
    {
        ConfigureAdminManag(modelBuilder);
    }

    private static void ConfigureAdminManag(ModelBuilder modelBuilder)
    {
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
            b.Property(cs => cs.Gender).HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)
            ).HasMaxLength(UserEntityConsts.MaxGenderLength);
        });
    }

    /// <summary>
    /// Course
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ConfigureCourseManagement(this ModelBuilder modelBuilder)
    {
        ConfigureCourseManag(modelBuilder);
    }

    private static void ConfigureCourseManag(ModelBuilder modelBuilder)
    {
        //MoocCourseInstance
        //modelBuilder.Entity<MoocCourseInstance>(e =>
        //{
        //    e.ToTable(TablePrefix + "MoocCourseInstance");
        //});

        //MoocEnrollment
        modelBuilder.Entity<MoocEnrollment>(b =>
        {
            b.ToTable(TablePrefix + "Enrollment");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.CourseInstanceId).IsRequired();
            b.Property(cs => cs.EnrollmentStatus).HasConversion(
                 v => v.ToString(),
                 v => (EnrollmentStatus)Enum.Parse(typeof(EnrollmentStatus), v)
             ).HasMaxLength(20);
            b.Property(e => e.EnrollStartDate).IsRequired();
            b.Property(e => e.EnrollEndDate).IsRequired();
            b.Property(e => e.MaxStudents)
                .IsRequired()
                .HasAnnotation("Range", new { Min = EnrollementEntityConsts.MinStudents, Max = EnrollementEntityConsts.MaxStudents });
            b.Property(e => e.CreatedByUserId).IsRequired();
            b.Property(e => e.UpdatedByUserId).IsRequired();
            b.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(e => e.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");
        });
    }
}
