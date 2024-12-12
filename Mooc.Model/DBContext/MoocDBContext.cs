namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    //public DbSet<MoocCourseInstance> MoocCourseInstances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //extension method
        modelBuilder.ConfigureAdminManagement();

        modelBuilder.ConfigureCourseManagement();
    }
}
