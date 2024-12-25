using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<ChoiceQuestion> ChoiceQuestion { get; set; }
    public DbSet<JudgementQuestion> JudgementQuestion { get; set; }
    public DbSet<ShortAnsQuestion> ShortAnsQuestion { get; set; }
    public DbSet<Option> Option { get; set; }
    public DbSet<QuestionType> QuestionType { get; set; }
    public DbSet<Exam> Exam { get; set; }
    public DbSet<ExamQuestion> ExamQuestion { get; set; }
    public DbSet<ExamPublish> ExamPublish { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //extension method
        modelBuilder.ConfigureAdminManagement();

        modelBuilder.ConfigureExamManagement();
    }
}
