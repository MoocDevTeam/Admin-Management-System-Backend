using Mooc.Shared.Entity.Admin;
using Mooc.Model.Entity.Course;
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
        ConfigureMoocTeacher(modelBuilder);
    }

    private static void ConfigureCourseManag(ModelBuilder modelBuilder)
    {
        //MoocCourseInstance
        //modelBuilder.Entity<MoocCourseInstance>(e =>
        //{
        //    e.ToTable(TablePrefix + "MoocCourseInstance");
        //});
    }

    /// <summary>
    /// Teacher
    /// <summary>
    private static void ConfigureMoocTeacher(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teacher>(b =>
        {
            b.ToTable("Teacher");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(cs => cs.Title).IsRequired().HasMaxLength(TeacherEntityConsts.MaxTitleLength);
            b.Property(cs => cs.Department).IsRequired().HasMaxLength(TeacherEntityConsts.MaxDepartmentLength);
            b.Property(cs => cs.Office).IsRequired().HasMaxLength(TeacherEntityConsts.MaxOfficeLength);
            b.Property(cs => cs.Introduction).HasMaxLength(TeacherEntityConsts.MaxIntroductionLength);
            b.Property(cs => cs.Expertise).IsRequired().HasMaxLength(TeacherEntityConsts.MaxExpertiseLength);
            
            //Foreign configuration temperarily use <User> until <MoocUser is created>
            b.HasOne<User>(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<User>(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
