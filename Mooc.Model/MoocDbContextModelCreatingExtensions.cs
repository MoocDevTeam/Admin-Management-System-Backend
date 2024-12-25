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
            b.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");//for SQLite


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
            // b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.courseId).IsRequired();
            b.Property(x => x.examTitle).HasMaxLength(ExamEntityConsts.MaxExamTitleLength);
            b.Property(x => x.remark).HasMaxLength(ExamEntityConsts.MaxRemarklLength);
            b.Property(x => x.examinationTime).IsRequired().HasMaxLength(ExamEntityConsts.MaxExaminationTimeLength);
            b.Property(x => x.createdAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");//for SQLite
            b.Property(x => x.createdByUserId).IsRequired();
            // updatedByUserId，updatedAt do not need setting?
            b.Property(x => x.updatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.autoOrManual).IsRequired().HasConversion(
                v => v.ToString(),
                v => (QuestionUpload)Enum.Parse(typeof(QuestionUpload), v));
            b.Property(x => x.totalScore).IsRequired().HasMaxLength(ExamEntityConsts.MaxTotalScoreLength);
            b.Property(x => x.timePeriod).IsRequired().HasMaxLength(ExamEntityConsts.MaxTimePeriodLength);
        });
    }
    private static void ConfigureExamQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamQuestion>(b =>
        {
            b.ToTable(TablePrefix + "Exam Question");
            b.HasKey(x => x.Id);
            // b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.examId).IsRequired();
            b.Property(x => x.questionId).IsRequired();
            b.Property(x => x.marks).IsRequired().HasMaxLength(ExamQuestionEntityConsts.MaxMarksLength);
            b.Property(x => x.questionOrder).IsRequired().HasMaxLength(ExamQuestionEntityConsts.MaxQuestionOrderLength);
            b.Property(x => x.createdAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.updatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }

    private static void ConfigureExamPublish(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamPublish>(b =>
        {
            b.ToTable(TablePrefix + "Exam Publish");
            b.HasKey(x => x.Id);
            // b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.examId).IsRequired();
            b.Property(x => x.publishedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.closeAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.courseInstanceId).IsRequired();
        });
    }

    
}
