namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<MoocUser> MoocUsers { get; set; }
    public DbSet<MoocUserRole> MoocUserRoles { get; set; }

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
