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
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<TeacherCourseInstance> TeacherCourseInstances { get; set; }
    public DbSet<Category> Category { get; set; }


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

        modelBuilder.ConfigureCourseManagement();

    }

}
