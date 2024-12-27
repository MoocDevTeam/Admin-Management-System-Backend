using Mooc.Shared.Entity.Admin;
using Mooc.Shared.Entity.Exam;

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
            b.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite
        });
    }

    public static void ConfigureExamManagement(this ModelBuilder modelBuilder)
    {
        ConfigureExam(modelBuilder);
        ConfigureExamQuestion(modelBuilder);
        ConfigureExamPublish(modelBuilder);
    }

    private static void ConfigureExam(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(b =>
        {
            b.ToTable(TablePrefix + "Exam");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
         /* b.HasOne<Course>()
                .WithMany()
                .HasForeignKey(x => x.CourseId); */
            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId);
            b.HasOne<User>()
            // HasMany wait future needs
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId);
            b.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite
            b.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite
            b.Property(x => x.ExamTitle)
                .HasMaxLength(ExamEntityConsts.MaxExamTitleLength);
            b.Property(x => x.Remark)
                .HasMaxLength(ExamEntityConsts.MaxRemarklLength);
            b.Property(x => x.ExaminationTime)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxExaminationTimeLength);
            b.Property(x => x.AutoOrManual)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (QuestionUpload)Enum.Parse(typeof(QuestionUpload), v)
                );
            b.Property(x => x.TotalScore)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxTotalScoreLength);
            b.Property(x => x.TimePeriod)
                .IsRequired()
                .HasMaxLength(ExamEntityConsts.MaxTimePeriodLength);
        });
    }
    private static void ConfigureExamQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ExamQuestion");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.HasOne<Exam>()
                .WithMany()
                .HasForeignKey(x => x.ExamId);
            /*          b.HasOne<ChoiceQuestion>()
                           .WithMany()
                               .HasForeignKey(x => x.QuestionId);
                        b.HasOne<JudgementQuestion>()
                           .WithMany()
                               .HasForeignKey(x => x.QuestionId);
                        b.HasOne<ShortAnsQuestion>()
                           .WithMany()
                               .HasForeignKey(x => x.QuestionId);*/
            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId);
            b.HasOne<User>()
            // HasMany wait future needs
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId);
            b.Property(x => x.Marks)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxMarksLength);
            b.Property(x => x.QuestionOrder)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxQuestionOrderLength);
            b.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }

    private static void ConfigureExamPublish(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamPublish>(b =>
        {
            b.ToTable(TablePrefix + "ExamPublish");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.HasOne(x => x.Exam) // Navigation property in ExamPublish
                .WithOne(x => x.ExamPublish) // Navigation property in Exam
                .HasForeignKey<ExamPublish>(x => x.ExamId); // ExamPublish.ExamId is the FK to Exam.Id
            b.HasOne(x => x.PublishedByUser); // default
            b.Property(x => x.PublishedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.CloseAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }

    
}
