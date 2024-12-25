using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<MoocUser> MoocUsers { get; set; }
    public DbSet<MoocUserRole> MoocUserRoles { get; set; }
    public DbSet<ChoiceQuestion> ChoiceQuestions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<ExamPublish> ExamPublishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //extension method
        modelBuilder.ConfigureAdminManagement();

        modelBuilder.ConfigureExamManagement();
    }
}
