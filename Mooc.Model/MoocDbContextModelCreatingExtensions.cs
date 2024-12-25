using Mooc.Shared.Entity.Admin;
using Mooc.Model.Entity.Course;
using Mooc.Shared.Entity.Course;
using Mooc.Shared.Enum;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            // Explicit relationship to MoocCourse for CreatedCourses
            b.HasMany(u => u.CreatedCourses)
                .WithOne(c => c.CreatedByUser)
                .HasForeignKey(c => c.CreatedByUserId)  // Foreign key property
                .OnDelete(DeleteBehavior.Restrict);  // Delete behavior

            // Explicit relationship to MoocCourse for UpdatedCourses
            b.HasMany(u => u.UpdatedCourses)
                .WithOne(c => c.UpdatedByUser)
                .HasForeignKey(c => c.UpdatedByUserId)  // Foreign key property
                .OnDelete(DeleteBehavior.Restrict);  // Delete behavior
        });
    }

    /// <summary>
    /// Course
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ConfigureCourseManagement(this ModelBuilder modelBuilder)
    {
        ConfigureCourseInstance(modelBuilder);
        ConfigureTeacher(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureEnrollment(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureSessionManage(modelBuilder);
        ConfigureMedia(modelBuilder);
        ConfigureTeacherCourseInstance(modelBuilder);
    }

    private static void ConfigureCourseInstance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseInstance>(c =>
        {
            c.ToTable(TablePrefix + "CourseInstances");
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).ValueGeneratedNever();
            c.Property(x => x.MoocCourseId).IsRequired();
            c.Property(x => x.TotalSessions).IsRequired();
            c.Property(x => x.Status).HasConversion(
                v => v.ToString(),
                v => (CourseInstanceStatus)Enum.Parse(typeof(CourseInstanceStatus), v))
            .IsRequired()
            .HasMaxLength(CourseInstanceEntityConsts.MaxStatusLength);
            c.Property(x => x.Permission).HasConversion(
                v => v.ToString(),
                v => (CourseInstancePermission)Enum.Parse(typeof(CourseInstancePermission), v))
            .IsRequired()
            .HasMaxLength(CourseInstanceEntityConsts.MaxPermissionLength);
            c.Property(x => x.StartDate).IsRequired();
            c.Property(x => x.EndDate).IsRequired();
            c.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            c.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            c.Property(x => x.CreatedByUserId).IsRequired();
            c.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.CreatedCourseInstances)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            c.Property(x => x.UpdatedByUserId).IsRequired();
            c.HasOne(x => x.UpdatedByUser)
                .WithMany(u => u.UpdatedCourseInstances)
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // One to One: CourseInstance->MoocCourse
            c.HasOne(x => x.MoocCourse)
                .WithOne() //.WithOne(mc => mc.CourseInstance) add 
                .HasForeignKey<CourseInstance>(x => x.MoocCourseId)
                .OnDelete(DeleteBehavior.Cascade);
            // One to Many: CourseInstance->Sessions
            //c.HasMany(x => x.Sessions)
            //.WithOne(s => s.CourseInstance)
            // .HasForeignKey(s => s.CourseInstanceId)
            //  .OnDelete(DeleteBehavior.Cascade);
            //One to Many: CourseInstance->TeacherCourseInstances
            c.HasMany(x => x.TeacherCourseInstances)
                .WithOne(tci => tci.CourseInstance)
                .HasForeignKey(tci => tci.CourseInstanceId)
                .OnDelete(DeleteBehavior.Restrict);
            // One-to-One: CourseInstance -> Enrollment
            //c.HasOne(x => x.Enrollment)
            //    .WithOne(e => e.CourseInstance)
            //    .HasForeignKey<Enrollment>(e => e.CourseInstanceId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // They will be moved to MoocUser Configuration later
            // One to Many: MoocUser->CreatedCourseInstances
            //b.HasMany(cs => cs.CreatedCourseInstances)
            //            .WithOne(cci => cci.CreatedByUser)
            //            .HasForeignKey(cci => cci.CreatedByUserId)
            //            .OnDelete(DeleteBehavior.Restrict);
            // One to Many: MoocUser->UpdatedCourseInstances
            //b.HasMany(cs => cs.UpdatedCourseInstances)
            //            .WithOne(uci => uci.UpdatedByUser)
            //            .HasForeignKey(uci => uci.UpdatedByUserId)
            //            .OnDelete(DeleteBehavior.Restrict);
        });
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

            //Set create or update time by Datebase itself
            b.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(e => e.UpdatedAt)
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()");

            b.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();

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
            b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedCourses)  // User has many CreatedCourses
            .HasForeignKey(x => x.CreatedByUserId)  // Foreign key property
            .OnDelete(DeleteBehavior.Restrict);  // Delete behavior

            b.HasOne(x => x.UpdatedByUser)
            .WithMany(u => u.UpdatedCourses)  // User has many UpdatedCourses
            .HasForeignKey(x => x.UpdatedByUserId)  // Foreign key property
            .OnDelete(DeleteBehavior.Restrict);  // Delete behavior

            // b.HasOne(x => x.Category)  // One Category
            // .WithMany(c => c.Courses) // Many Courses
            // .HasForeignKey(x => x.CategoryId) // Foreign Key in MoocCourse
            // .OnDelete(DeleteBehavior.Restrict); // Restrict delete if needed
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

            //Set create or update time by Datebase itself
            b.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(e => e.UpdatedAt)
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()");

            b.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();

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

            // Explicit relationship to MoocCourse for UpdatedCourses
            b.HasMany(u => u.Courses)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)  // Foreign key property
            .OnDelete(DeleteBehavior.Restrict);  // Delete behavior
        });
    }

    private static void ConfigureEnrollment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(b =>
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
                .HasMaxLength(300);
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
    ///Course-Session
    private static void ConfigureSessionManage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>(b =>
        {
            b.ToTable("Session");  // Set the table name as "Session"

            b.HasKey(e => e.Id);  // Set the primary key to be the Id field

            // Configure field properties
            b.Property(x => x.Title)
                .IsRequired()  // Mark Title as required (not nullable)
                .HasMaxLength(SessionEntityConsts.MaxTitleLength);  // Set the maximum length to 50

            b.Property(x => x.Description)
                .HasMaxLength(SessionEntityConsts.MaxDescriptionLength);  // Set the maximum length to 255

            b.Property(x => x.Order)
                .IsRequired();  // Mark Order as required (not nullable)

            b.Property(x => x.CreatedByUserId)
                .IsRequired();  // Mark CreatedByUserId as required (not nullable)

            b.Property(x => x.UpdatedByUserId)
                .IsRequired(false);  // Mark UpdatedByUserId as optional (nullable)

            b.Property(x => x.CreatedAt)
                .IsRequired()  // Mark CreatedAt as required (not nullable)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");  // Set the default value to current timestamp

            b.Property(x => x.UpdatedAt)
                .IsRequired(false)  // Mark UpdatedAt as optional (nullable)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");  // Set the default value to current timestamp

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

    private static void ConfigureTeacherCourseInstance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeacherCourseInstance>(b =>
            {
                b.ToTable("TeacherCourseInstance");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedNever();
                b.Property(x => x.PermissionType).HasConversion(
                    v => v.ToString(),
                    v => (TeacherCourseInstancePermissionType)Enum.Parse(typeof(TeacherCourseInstancePermissionType), v));
                b.Property(e => e.CreatedByUserId).IsRequired();
                b.Property(e => e.UpdatedByUserId);

                //Set create or update time by Datebase itself
                b.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
                b.Property(e => e.UpdatedAt)
                    .IsRequired(false)
                    .HasDefaultValueSql("GETDATE()");

                b.Property (e => e.UpdatedAt).ValueGeneratedOnUpdate();

                //Foreign Keys
                b.HasOne<User>(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<User>(x => x.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<Teacher>(x => x.Teacher)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<CourseInstance>(x => x.CourseInstance)
                    .WithMany()
                    .HasForeignKey(x => x.CourseInstanceId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        );
    }

    private static void ConfigureMedia(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>(b =>
        {
            b.ToTable("Media");

            // Primary key
            b.HasKey(x => x.Id);
            b.Property(e => e.Id).ValueGeneratedNever();

            // Properties
            b.Property(cs => cs.UploaderId).IsRequired();
            b.Property(cs => cs.SessionId).IsRequired();
            b.Property(cs => cs.FileType)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (FileTypeEnum)Enum.Parse(typeof(FileTypeEnum), v)
                );
            b.Property(cs => cs.FileName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");
            b.Property(cs => cs.FilePath)
                .IsRequired()
                .HasColumnType("text");
            b.Property(cs => cs.ThumbnailPath)
                .IsRequired()
                .HasColumnType("text");
            b.Property(cs => cs.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(cs => cs.UpdatedAt)
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()");
            b.Property(cs => cs.ApprovalStatus)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (ApprovalStatusEnum)Enum.Parse(typeof(ApprovalStatusEnum), v)
                );

            // Relationships
            b.HasOne<User>(x => x.Uploader)
                .WithMany()
                .HasForeignKey(x => x.UploaderId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<Session>(x => x.Session)
                .WithMany()
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
