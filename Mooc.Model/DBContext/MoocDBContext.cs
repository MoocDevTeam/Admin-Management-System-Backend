using Mooc.Model.Entity.Course;

namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<MoocCourse> MoocCourses { get; set; }
    public DbSet<Session> Session { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<CourseInstance> CourseInstances { get; set; }
    public DbSet<Teacher> MoocTeachers { get; set; }
    public DbSet<Enrollment> MoocEnrollment { get; set; }
    public DbSet<TeacherCourseInstance> TeacherCourseInstances { get; set; }
    public DbSet<Category> Category { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //extension method
        modelBuilder.ConfigureAdminManagement();

        modelBuilder.ConfigureCourseManagement();
    }

}
