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
        ConfigureTeacher(modelBuilder);
        ConfigureCategory(modelBuilder);
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
    private static void ConfigureTeacher(ModelBuilder modelBuilder)
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

    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MoocCourse>(b =>
        {
            b.ToTable("MoocCourses");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(cs => cs.Title).IsRequired().HasMaxLength(CourseEntityConsts.MaxTitleLength);
            b.Property(cs => cs.CourseCode).IsRequired().HasMaxLength(CourseEntityConsts.MaxCourseCodeLength);
            b.Property(cs => cs.CoverImage).IsRequired();
            b.Property(cs => cs.Description).HasMaxLength(CourseEntityConsts.MaxDescriptionLength);

            // //Foreign configuration temperarily use <User> until <MoocUser is created>
            // b.HasOne<User>(x => x.CreatedByUser)
            // .WithMany()
            // .HasForeignKey(x => x.CreatedByUserId)
            // .OnDelete(DeleteBehavior.Cascade);

            // b.HasOne<User>(x => x.UpdatedByUser)
            // .WithMany()
            // .HasForeignKey(x => x.UpdatedByUserId)
            // .OnDelete(DeleteBehavior.Cascade);
        });
    }
    ///<summary>
    /// Category
    /// <summary>
    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(b =>
        {
            b.ToTable("Category");
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(cs => cs.CategoryName).IsRequired().HasMaxLength(CategoryEntityConsts.MaxCategoryNameLength);
            b.Property(cs => cs.Description).IsRequired().HasMaxLength(CategoryEntityConsts.MaxDescriptionLength);
            b.Property(cs => cs.IconUrl).IsRequired().HasMaxLength(CategoryEntityConsts.MaxIconUrlLength);
            b.Property(cs => cs.CreatedAt).IsRequired();
            b.Property(cs => cs.UpdatedAt);

            // foreign keys 
            b.HasOne(x => x.ParentCategory)
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

            //foreign keys to User class
            b.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
