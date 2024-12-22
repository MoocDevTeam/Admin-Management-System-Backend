using Mooc.Shared.Entity.Admin;
using Mooc.Shared.Entity.ExamManagement;

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
        ConfigureChoiceQuestion(modelBuilder);
        ConfigureOption(modelBuilder);
    }

    private static void ConfigureChoiceQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChoiceQuestion>(c =>
        {
            c.ToTable(TablePrefix + "ChoiceQuestion");
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).ValueGeneratedNever();
            /* c.HasOne(QuestionType);*/
            c.HasMany(x => x.Options);
            /* c.HasOne(Course);*/
            c.Property(x => x.CorrectAnswer).IsRequired().HasMaxLength(ChoiceQuestionEntityConsts.MaxCorrectAnswerLength);
            /* c.HasOne(x => x.CreatedByUser);*/
            c.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite*/
            /* c.HasMany(x => x.UpdatedByUsers);*/
            c.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite*/
            c.Property(x => x.QuestionBody).IsRequired().HasMaxLength(ChoiceQuestionEntityConsts.MaxQuestionBodyLength);
            c.Property(x => x.QuestionTitle).HasMaxLength(ChoiceQuestionEntityConsts.MaxQuestionTitleLength);
            c.Property(x => x.Marks).HasMaxLength(ChoiceQuestionEntityConsts.MaxMarkLength).HasDefaultValue(5);
        });
    }

    private static void ConfigureOption(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(c =>
        {
            c.ToTable(TablePrefix + "Option");
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).ValueGeneratedNever();
            c.HasMany(x => x.ChoiceQuestions);
            c.Property(x => x.OptionOrder).HasMaxLength(OptionEntityConsts.MaxQuestionOrderLength);
            c.Property(x => x.OptionValue).HasMaxLength(OptionEntityConsts.MaxOptionValueLength);
           /* c.HasOne(x => x.CreatedByUser);*/
            c.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite*/
           /* c.HasMany(x => x.UpdatedByUsers);*/
            c.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite*/
        });
    }

}
