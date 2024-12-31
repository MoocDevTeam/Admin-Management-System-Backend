using Mooc.Model.Entity.Course;

namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Carousel> Carousels { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RoleMenu> RoleMenus { get; set; }
    public DbSet<Role> Roles { get; set; }
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
