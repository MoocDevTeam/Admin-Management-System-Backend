using Mooc.Shared.Entity.Admin;
using Mooc.Shared.Entity.ExamManagement;
using Mooc.Shared.Entity.Course;
using Mooc.Model.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



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
            b.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");//for SQLite
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
            b.ConfigureAudit();
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
           // b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.HasKey(ur => new { ur.UserId, ur.RoleId });
            b.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId);
        });


    }

    private static void ConfigureCarousel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carousel>(b =>
        {

            b.HasKey(x => x.Id);
            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(CarouselEntityConsts.MaxTitleLength);
            b.Property(x => x.ImageUrl).IsRequired();
            b.Property(x => x.RedirectUrl)
                .HasMaxLength(CarouselEntityConsts.MaxRedirectUrlLength);
            b.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(CarouselEntityConsts.DefaultIsActive);
            b.Property(x => x.UpdatedAt).IsRequired();
            b.Property(x => x.StartDate).IsRequired();
            b.Property(x => x.EndDate).IsRequired();
            b.ToTable("Carousel", t =>
            {
                t.HasCheckConstraint("CK_Carousel_Position_Range", "[Position] BETWEEN 1 AND 6");
            });
            b.Property(x => x.Position)
                .IsRequired();
            b.HasIndex(x => x.Position).IsUnique();
            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId);
            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId);
        });
    }

    private static void ConfigureComment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(b =>
        {
            b.ToTable("Comment");
            b.HasKey(c => c.Id);
            b.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);
            b.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            b.Property(c => c.IsFlagged)
                .IsRequired()
                .HasDefaultValue(false);
            b.HasOne(c => c.MoocCourse)
                .WithMany()
                .HasForeignKey(c => c.MoocCourseId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.Session)
                .WithMany()
                .HasForeignKey(c => c.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
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

    /// <summary>
    /// Configures the CourseInstance entity in the database model
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder used for entity configuration</param>
    private static void ConfigureCourseInstance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseInstance>(c =>
        {
            c.ToTable(TablePrefix + "CourseInstances");
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).ValueGeneratedNever();
            c.Property(x => x.Description).IsRequired();
            c.Property(x => x.MoocCourseId).IsRequired();
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

            c.ConfigureAudit();

            //Many to One
            c.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            //Many to One
            c.HasOne(x => x.UpdatedByUser)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Many to One: CourseInstance->MoocCourse
            c.HasOne(x => x.MoocCourse)
                .WithMany(mc => mc.CourseInstances)
                .HasForeignKey(x => x.MoocCourseId)
                .OnDelete(DeleteBehavior.Cascade); //CourseInstance will be deleted automatically
            // One to Many: CourseInstance->Sessions
            c.HasMany(x => x.Sessions)
                .WithOne(s => s.CourseInstance)
                .HasForeignKey(s => s.CourseInstanceId)
                .OnDelete(DeleteBehavior.Cascade);
            //One to Many: CourseInstance->TeacherCourseInstances
            c.HasMany(x => x.TeacherCourseInstances)
                .WithOne(tci => tci.CourseInstance)
                .HasForeignKey(tci => tci.CourseInstanceId)
                .OnDelete(DeleteBehavior.Restrict);
            // One-to-One: CourseInstance -> Enrollment
            c.HasOne(x => x.Enrollment)
                .WithOne()
                .HasForeignKey<Enrollment>(e => e.CourseInstanceId)
                .OnDelete(DeleteBehavior.Cascade);
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
            b.Property(t => t.Title).IsRequired().HasMaxLength(TeacherEntityConsts.MaxTitleLength);
            b.Property(t => t.Department).IsRequired().HasMaxLength(TeacherEntityConsts.MaxDepartmentLength);
            b.Property(t => t.Office).IsRequired().HasMaxLength(TeacherEntityConsts.MaxOfficeLength);
            b.Property(t => t.Introduction).HasMaxLength(TeacherEntityConsts.MaxIntroductionLength);
            b.Property(t => t.Expertise).IsRequired().HasMaxLength(TeacherEntityConsts.MaxExpertiseLength);
            b.Property(t => t.DisplayName).IsRequired().HasMaxLength(TeacherEntityConsts.MaxDisplayNameLength);
            b.ConfigureAudit();


            b.HasOne<User>(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

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
            b.ConfigureAudit();

            //Foreign configuration temperarily use <User> until <MoocUser is created>
            b.HasOne<User>(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Category)  // One Category
            .WithMany(c => c.Courses) // Many Courses
            .HasForeignKey(x => x.CategoryId) // Foreign Key in MoocCourse
            .OnDelete(DeleteBehavior.Restrict); // Restrict delete if needed
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
            b.Property(cs => cs.IconUrl).HasMaxLength(CategoryEntityConsts.MaxIconUrlLength);

            // foreign keys
            b.HasOne(x => x.ParentCategory)
            .WithMany(x =>x.ChildrenCategories)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

            //foreign keys to User class
            b.HasOne<User>(x => x.CreatedByUser)
             .WithMany()
             .HasForeignKey(x => x.CreatedByUserId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<User>(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

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
    ///Course-Session
    private static void ConfigureSessionManage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>(b =>
        {
            b.ToTable("Session");  // Set the table name
            b.HasKey(e => e.Id);  // primary key
            b.Property(x => x.Id).ValueGeneratedNever();

            // Properties
            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(SessionEntityConsts.MaxTitleLength); // 50

            b.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(SessionEntityConsts.MaxDescriptionLength);

            b.Property(x => x.Order)
                .IsRequired();

            b.Property(x => x.CourseInstanceId)
              .IsRequired();

            // b.Property(x => x.CreatedByUserId)
            //     .IsRequired();

            // b.Property(x => x.UpdatedByUserId)
            //     .IsRequired(false);

            // b.Property(x => x.CreatedAt)
            //     .IsRequired()
            //     .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // b.Property(x => x.UpdatedAt)
            //     .IsRequired()
            //     .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Relationships
            b.HasOne<User>(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<User>(x => x.UpdatedByUser)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<CourseInstance>(x => x.CourseInstance)
                .WithMany(s => s.Sessions)
                .HasForeignKey(s => s.CourseInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany<Media>(x => x.Sessionmedia)
                .WithOne(s => s.Session)
                .HasForeignKey(s => s.SessionId)
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
            b.ConfigureAudit();

            //Set create or update time by Datebase itself
            //b.Property(e => e.CreatedAt)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");
            //b.Property(e => e.UpdatedAt)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");

            b.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();

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
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<CourseInstance>(x => x.CourseInstance)
                .WithMany(x => x.TeacherCourseInstances)
                .HasForeignKey(x => x.CourseInstanceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
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
            b.Property(cs => cs.FileType)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (MediaFileType)Enum.Parse(typeof(MediaFileType), v)
                );
            b.Property(cs => cs.FileName)
                .IsRequired()
                .HasMaxLength(MediaEntityConsts.MaxFileNameLength); //255
            b.Property(cs => cs.FilePath)
                .IsRequired();
            b.Property(cs => cs.ThumbnailPath)
                .IsRequired();
            // b.Property(cs => cs.CreatedAt)
            //     .IsRequired()
            //     .HasDefaultValueSql("GETDATE()");
            // b.Property(cs => cs.UpdatedAt)
            //     .IsRequired()
            //     .HasDefaultValueSql("GETDATE()");
            b.Property(cs => cs.ApprovalStatus)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (MediaApprovalStatus)Enum.Parse(typeof(MediaApprovalStatus), v)
                );
            b.Property(cs => cs.SessionId).IsRequired();
            // b.Property(x => x.CreatedByUserId)
            //     .IsRequired();
            // b.Property(x => x.UpdatedByUserId)
            //     .IsRequired(false);

            // Relationships
            b.HasOne<User>(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<User>(x => x.UpdatedByUser)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<Session>(x => x.Session)
                .WithMany(x => x.Sessionmedia)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }


    public static void ConfigureExamManagement(this ModelBuilder modelBuilder)
    {
        ConfigureChoiceQuestion(modelBuilder);
        ConfigureJudgementQuestion(modelBuilder);
        ConfigureShortAnsQuestion(modelBuilder);
        ConfigureOption(modelBuilder);
        ConfigureQuestionType(modelBuilder);
        ConfigureExam(modelBuilder);
        ConfigureExamQuestion(modelBuilder);
        ConfigureExamPublish(modelBuilder);
    }

    private static void ConfigureChoiceQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChoiceQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ChoiceQuestion", t =>
            {
                t.HasCheckConstraint("CK_ChoiceQuestion_Marks",
                    $"Marks >= {BaseQuestionEntityConsts.MinMarksValue} AND Marks <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(cq => cq.Id);
            b.Property(cq => cq.Id).ValueGeneratedNever();
            b.Property(cq => cq.QuestionBody)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionBodyLength);
            b.Property(cq => cq.QuestionTitle)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionTitleLength);
            b.Property(cq => cq.CorrectAnswer)
                .IsRequired()
                .HasMaxLength(ChoiceQuestionEntityConsts.MaxCorrectAnswerLength);
            b.Property(cq => cq.Marks)
               .IsRequired();
            b.HasOne(cq => cq.QuestionType)
                 .WithMany(qt => qt.ChoiceQuestions)
                 .HasForeignKey(cq => cq.QuestionTypeId);
            b.HasMany<Option>()
                 .WithOne(o => o.ChoiceQuestion)
                 .HasForeignKey(o => o.ChoiceQuestionId);
            b.HasOne(cq => cq.CourseInstance)
                   .WithMany()
                   .HasForeignKey(cq => cq.CourseId);
            b.ConfigureAudit();
        });
    }

    private static void ConfigureOption(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(b =>
        {
            b.ToTable(TablePrefix + "Option");
            b.HasKey(o => o.Id);
            b.Property(o => o.Id)
                .ValueGeneratedNever();  // Prevent auto-increment, we'll use UUID/Snowflake
            b.HasOne(o => o.ChoiceQuestion)
                .WithMany(cq => cq.Option)
                .HasForeignKey(o => o.ChoiceQuestionId);

            b.Property(o => o.OptionOrder)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxOrderLength);
            b.Property(o => o.OptionValue)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxOrderLength);
            b.Property(o => o.ErrorExplanation)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxErrorExplanationLength);
            b.ConfigureAudit();
        });
    }

    private static void ConfigureJudgementQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JudgementQuestion>(b =>
        {
            b.ToTable(TablePrefix + "JudgementQuestion", t =>
            {
                t.HasCheckConstraint("CK_JudgementQuestionn_Marks",
                    $"Marks >= {BaseQuestionEntityConsts.MinMarksValue} AND Marks <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(jq => jq.Id);
            b.Property(jq => jq.Id)
                .ValueGeneratedNever();

            b.Property(jq => jq.QuestionBody)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionBodyLength);
            b.Property(jq => jq.QuestionTitle)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionTitleLength);
            b.Property(jq => jq.CorrectAnswer)
                .IsRequired()
                .HasDefaultValue(JudgementQuestionEntityConsts.DefaultCorrectAnswer);
            b.Property(jq => jq.Marks)
                .IsRequired();
            b.HasOne(jq => jq.QuestionType)
                .WithMany(qt => qt.JudgementQuestions)
                .HasForeignKey(cq => cq.QuestionTypeId);
            b.HasOne(jq => jq.CourseInstance)
             .WithMany()
             .HasForeignKey(jq => jq.CourseId);
            b.ConfigureAudit();
        });
    }

    private static void ConfigureShortAnsQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortAnsQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ShortAnsQuestion", t =>
            {
                t.HasCheckConstraint("CK_ShortAnsQuestion_Marks",
                    $"Marks >= {BaseQuestionEntityConsts.MinMarksValue} AND Marks <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(saq => saq.Id);
            b.Property(saq => saq.Id).ValueGeneratedNever();
            b.Property(saq => saq.QuestionBody)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionBodyLength);
            b.Property(saq => saq.QuestionTitle)
                .IsRequired()
                .HasMaxLength(BaseQuestionEntityConsts.MaxQuestionTitleLength);
            b.Property(saq => saq.ReferenceAnswer)
                .IsRequired()
                .HasMaxLength(ShortAnsQuestionEntityConsts.MaxReferenceAnsLength);
            b.Property(saq => saq.Marks)
                .IsRequired();
            b.HasOne(saq => saq.QuestionType)
                .WithMany(qt => qt.ShortAnsQuestions)
                .HasForeignKey(saq => saq.QuestionTypeId);
            b.HasOne(saq => saq.CourseInstance)
                 .WithMany()
                 .HasForeignKey(saq => saq.CourseId);
            b.ConfigureAudit();
        });
    }
    private static void ConfigureQuestionType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionType>(b =>
        {
            b.ToTable(TablePrefix + "QuestionType");
            b.HasKey(qt => qt.Id);
            b.Property(qt => qt.Id)
                .ValueGeneratedNever();
            b.Property(qt => qt.QuestionTypeName)
                .IsRequired()
                .HasMaxLength(QuestionTypeEntityConsts.MaxQuestionTypeNameLength);
        });
    }

    private static void ConfigureExam(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(b =>
        {
            b.ToTable(TablePrefix + "Exam");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.HasOne(e=>e.CourseInstance)
                .WithMany()
                .HasForeignKey(e => e.CourseId);
            b.Property(e => e.ExamTitle)
                .HasMaxLength(ExamEntityConsts.MaxExamTitleLength);
            b.Property(e => e.Remark)
                .HasMaxLength(ExamEntityConsts.MaxRemarklLength);
            b.Property(e => e.ExaminationTime)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxExaminationTimeLength);
            b.Property(e => e.AutoOrManual)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (QuestionUpload)Enum.Parse(typeof(QuestionUpload), v)
                );
            b.Property(e => e.TotalScore)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxTotalScoreLength);
            b.Property(e => e.TimePeriod)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxTimePeriodLength);
            b.HasMany(e => e.ExamQuestions)
              .WithOne(eq => eq.Exam)
              .HasForeignKey(e => e.ExamId);
            b.ConfigureAudit();
        });
    }
    private static void ConfigureExamQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ExamQuestion");
            b.HasKey(eq => eq.Id);
            b.Property(eq => eq.Id).ValueGeneratedNever();
            b.HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId);

            b.HasOne(m => m.ChoiceQuestion)
                .WithMany()
                .HasForeignKey(eq => eq.ChoiceQuestionId);
            b.HasOne(m => m.JudgementQuestion)
                .WithMany()
                .HasForeignKey(eq => eq.JudgementQuestionId);
            b.HasOne(m => m.ShortAnsQuestion)
                .WithMany()
                .HasForeignKey(eq => eq.ShortAnsQuestionId);
            b.Property(eq => eq.Marks)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxMarksLength);
            b.Property(eq => eq.QuestionOrder)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxQuestionOrderLength);
            b.ConfigureAudit();
        });
    }

    private static void ConfigureExamPublish(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamPublish>(b =>
        {
            b.ToTable(TablePrefix + "ExamPublish");
            b.HasKey(ep => ep.Id);
            b.Property(ep => ep.Id).ValueGeneratedNever();
            b.HasOne(ep => ep.Exam) // Navigation property in ExamPublish
                .WithOne(e => e.ExamPublish) // Navigation property in Exam
                .HasForeignKey<ExamPublish>(x => x.ExamId); // ExamPublish.ExamId is the FK to Exam.Id
            b.Property(ep => ep.CloseAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.ConfigureAudit();
        });
    }

    /// <summary>
    /// configuration for audit properties
    /// </summary>
    /// <typeparam name="T">your model<Teacher></typeparam>
    /// <param name="b">The delegate parameter passed to Entity<Teacher></param>
    public static void ConfigureAudit<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        if (b.Metadata.ClrType.IsSubclassOf(typeof(BaseEntityWithAudit)))
        {
            b.Property(nameof(BaseEntityWithAudit.CreatedByUserId)).
                IsRequired(true).
                HasColumnName(nameof(BaseEntityWithAudit.CreatedByUserId));

            b.Property(nameof(BaseEntityWithAudit.CreatedAt)).
                IsRequired(true).
                HasColumnName(nameof(BaseEntityWithAudit.CreatedAt));


            b.Property(nameof(BaseEntityWithAudit.UpdatedByUserId)).
               IsRequired(false).
               HasColumnName(nameof(BaseEntityWithAudit.UpdatedByUserId));

            b.Property(nameof(BaseEntityWithAudit.UpdatedAt)).
                IsRequired(false).
                HasColumnName(nameof(BaseEntityWithAudit.UpdatedAt));
        }
    }


}