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
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(cq => cq.CourseId);

            b.HasOne(cq => cq.CreatedByUser)
                //.WithMany(u => u.CreatedChoiceQuestions)
                .WithMany()
                .HasForeignKey(o => o.CreatedByUserId);

            b.HasOne<User>()
            //b.HasMany<User>()
                //.WithMany(u => u.UpdatedChoiceQuestions)
                .WithMany()
                //.UsingEntity(j =>
                //    j.ToTable("ChoiceQuestionUpdatedByUsers"));
                .HasForeignKey(cq => cq.UpdatedByUserId); 

            b.Property(cq => cq.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(cq => cq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
        });
    }

    private static void ConfigureOption(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(b =>
        {
            b.ToTable(TablePrefix + "Option");
            b.HasKey(o => o.Id);
            b.Property(o => o.Id)
                .ValueGeneratedNever();
            b.HasOne(o => o.ChoiceQuestion)
                .WithMany(cq => cq.Option)
                .HasForeignKey(o => o.ChoiceQuestionId);

            b.HasOne(o => o.CreatedByUser)
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
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(o => o.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
/*            b.Property(o => o.Field)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP)");*/
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
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(jq => jq.CourseId);

            b.HasOne(jq => jq.CreatedByUser)
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
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(jq => jq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
            b.Property(saq => saq.Id)
                .ValueGeneratedNever();
            //b.HasOne<Course>()
            //    .WithMany()
            //    .HasForeignKey(saq => saq.CourseId);

            b.HasOne(saq => saq.CreatedByUser)
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
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(saq => saq.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
            /* b.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId); */
            b.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId);
            b.HasOne<User>()
            // HasMany wait future needs
                .WithMany()
                .HasForeignKey(e => e.UpdatedByUserId);
            b.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite
            b.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); //for SQLite
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
            b.HasMany<ExamQuestion>()
               .WithOne(eq => eq.Exam)
               .HasForeignKey(e => e.ExamId);
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
                .WithMany(e => e.ExamQuestion)
                .HasForeignKey(eq => eq.ExamId);  // have side effects
            /* b.HasOne<Exam>()
                 .WithMany()
                 .HasForeignKey(x => x.ExamId);*/  // use this alternative method, because the above have side effects
                                                   // we can choose either have 3 columns (ChoiceQuestionId, JudgementQuestionId, ShortAnsQuestionId) or have 1 column (questionId)
            /*          b.HasOne<ChoiceQuestion>()
                            .WithMany()
                            .HasForeignKey(eq => eq.ChoiceQuestionId);
                        b.HasOne<JudgementQuestion>()
                            .WithMany()
                            .HasForeignKey(eq => eq.JudgementQuestionId);
                        b.HasOne<ShortAnsQuestion>()
                            .WithMany()
                            .HasForeignKey(eq => eq.ShortAnsQuestionId);*/
            // 3 columns (ChoiceQuestionId, JudgementQuestionId, ShortAnsQuestionId) like above commented
            b.Property(x => x.QuestionType)
                .IsRequired();
            // 1 column  (questionId) when use add controller, frontend need to send / backend controller need to accept 1 extra parameter QuestionType
            b.HasOne(eq => eq.CreatedByUser)
                .WithMany()
                .HasForeignKey(eq => eq.CreatedByUserId);
            b.HasOne<User>()
            // HasMany wait future needs
                .WithMany()
                .HasForeignKey(eq => eq.UpdatedByUserId);
            b.Property(eq => eq.Marks)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxMarksLength);
            b.Property(eq => eq.QuestionOrder)
                .IsRequired()
                .HasMaxLength(ExamQuestionEntityConsts.MaxQuestionOrderLength);
            b.Property(eq => eq.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(eq => eq.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
            b.HasOne(ep => ep.PublishedByUser); // default
            b.Property(ep => ep.PublishedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(ep => ep.CloseAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }

}
