using Mooc.Model.Entity.ExamManagement;
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
   

        //User
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

    public static void ConfigureExamManagement(this ModelBuilder modelBuilder)
    {
        ConfigureChoiceQuestion(modelBuilder);
        ConfigureJudgementQuestion(modelBuilder);
        ConfigureShortAnsQuestion(modelBuilder);
        ConfigureOption(modelBuilder);
        ConfigureQuestionType(modelBuilder);
    }

    public static void ConfigureChoiceQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChoiceQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ChoiceQuestion", t =>
            {
                t.HasCheckConstraint("CK_ChoiceQuestion_Marks",
                    $"[Marks] >= {BaseQuestionEntityConsts.MinMarksValue} AND [Marks] <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(cq => cq.Id);
            b.Property(cq => cq.Id)
                .ValueGeneratedNever();
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(cq => cq.CourseId);

            b.HasOne<User>()
                //.WithMany(u => u.CreatedChoiceQuestions)
                .WithMany()
                .HasForeignKey(cq => cq.CreatedByUserId);
            
            b.HasOne<User>()
            //b.HasMany<User>()
                //.WithMany(u => u.UpdatedChoiceQuestions)
                .WithMany()
                //.UsingEntity(j =>
                //    j.ToTable("ChoiceQuestionUpdatedByUsers"));
                .HasForeignKey(cq => cq.UpdatedByUserId); 

            b.Property(cq => cq.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(cq => cq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
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
            b.HasOne<QuestionType>()
                 .WithMany(qt => qt.ChoiceQuestions)
                 .HasForeignKey(cq => cq.QuestionTypeId);
            b.HasMany<Option>()
                 .WithOne(o => o.ChoiceQuestion)
                 .HasForeignKey(o => o.ChoiceQuestionId);
        });
    }

    public static void ConfigureOption(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(b =>
        {
            b.ToTable(TablePrefix + "Option");
            b.HasKey(o => o.Id);
            b.Property(o => o.Id)
                .ValueGeneratedNever();
            b.HasOne<ChoiceQuestion>()
                .WithMany()
                .HasForeignKey(o => o.ChoiceQuestionId);

            b.HasOne<User>()
                //.WithMany(u => u.CreatedChoiceQuestions)
                .WithMany()
                .HasForeignKey(o => o.CreatedByUserId);

            b.HasOne<User>()
                //b.HasMany<User>()
                //.WithMany(u => u.UpdatedChoiceQuestions)
                .WithMany()
                //.UsingEntity(j =>
                //    j.ToTable("OptionUpdatedByUsers"));
                .HasForeignKey(o => o.UpdatedByUserId);

            b.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(o => o.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(o => o.Field)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(o => o.OptionOrder)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxOrderLength);
            b.Property(o => o.OptionValue)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxOrderLength);
            b.Property(o => o.ErrorExplanation)
                .IsRequired()
                .HasMaxLength(OptionEntityConsts.MaxErrorExplanationLength);
        });
    }

    public static void ConfigureJudgementQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JudgementQuestion>(b =>
        {
            b.ToTable(TablePrefix + "JudgementQuestion", t =>
            {
                t.HasCheckConstraint("CK_JudgementQuestionn_Marks",
                    $"[Marks] >= {BaseQuestionEntityConsts.MinMarksValue} AND [Marks] <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(jq => jq.Id);
            b.Property(jq => jq.Id)
                .ValueGeneratedNever();
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(jq => jq.CourseId);

            b.HasOne<User>()
                //.WithMany(u => u.CreatedChoiceQuestions)
                .WithMany()
                .HasForeignKey(jq => jq.CreatedByUserId);

            b.HasOne<User>()
                //b.HasMany<User>()
                //.WithMany(u => u.UpdatedChoiceQuestions)
                .WithMany()
                //.UsingEntity(j =>
                //    j.ToTable("JudgementQuestionUpdatedByUsers"));
                .HasForeignKey(jq => jq.UpdatedByUserId);

            b.Property(jq => jq.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(jq => jq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
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
            b.HasOne<QuestionType>()
                 .WithMany(qt => qt.JudgementQuestions)
                 .HasForeignKey(jq => jq.QuestionTypeId);
        });
    }

    public static void ConfigureShortAnsQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortAnsQuestion>(b =>
        {
            b.ToTable(TablePrefix + "ShortAnsQuestion", t =>
            {
                t.HasCheckConstraint("CK_ShortAnsQuestion_Marks",
                    $"[Marks] >= {BaseQuestionEntityConsts.MinMarksValue} AND [Marks] <= {BaseQuestionEntityConsts.MaxMarksValue}");
            });

            b.HasKey(saq => saq.Id);
            b.Property(saq => saq.Id)
                .ValueGeneratedNever();
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(saq => saq.CourseId);

            b.HasOne<User>()
                //.WithMany(u => u.CreatedChoiceQuestions)
                .WithMany()
                .HasForeignKey(saq => saq.CreatedByUserId);

            b.HasOne<User>()
                //b.HasMany<User>()
                //.WithMany(u => u.UpdatedChoiceQuestions)
                .WithMany()
                //.UsingEntity(j =>
                //    j.ToTable("ShortAnsQuestionUpdatedByUsers"));
                .HasForeignKey(saq => saq.UpdatedByUserId);

            b.Property(saq => saq.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            b.Property(saq => saq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
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
            b.HasOne<QuestionType>()
                 .WithMany(qt => qt.ShortAnsQuestions)
                 .HasForeignKey(saq => saq.QuestionTypeId);
        });
    }
    public static void ConfigureQuestionType(ModelBuilder modelBuilder)
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
}
