namespace Mooc.Model.DBContext;

public class MoocDBContext : DbContext
{
    public MoocDBContext(DbContextOptions<MoocDBContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Carousel> Carousels { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RoleMenu> RoleMenus { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //extension method
        modelBuilder.ConfigureAdminManagement();

        //modelBuilder.Entity<Carousel>()
        //    .HasOne<User>()
        //    .WithMany()
        //    .HasForeignKey(c => c.CreatedByUserId);


        //modelBuilder.Entity<Carousel>()
        //    .HasOne<User>()
        //    .WithMany()
        //    .HasForeignKey(c => c.UpdatedByUserId);


        //modelBuilder.Entity<Comment>()
        //    .HasOne<Comment>()
        //    .WithMany()
        //    .HasForeignKey(c => c.ParentCommentId);
          
    }
}
