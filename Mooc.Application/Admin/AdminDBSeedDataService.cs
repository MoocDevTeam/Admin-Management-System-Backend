using Mooc.Application.Contracts;
using Mooc.Core.Utils;

namespace Mooc.Application.Admin
{
    public class AdminDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public AdminDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin", Password=BCryptUtil.HashPassword("123456"), Email="admin@demo.com", Age=30, Access=Access.admin, Gender=Gender.Male, Avatar="admin-avatar.png", CreatedAt=DateTime.Now, IsActive=true },
            new User(){Id=2, UserName="teacher1", Password=BCryptUtil.HashPassword("123456"), Email="teacher1@demo.com", Age=35, Access=Access.teacher, Gender=Gender.Male, Avatar="teacher1-avatar.png", CreatedAt=DateTime.Now.AddMinutes(1), IsActive=true },
            new User(){Id=3, UserName="teacher2", Password=BCryptUtil.HashPassword("123456"), Email="teacher2@demo.com", Age=32, Access=Access.teacher, Gender=Gender.Female, Avatar="teacher2-avatar.png", CreatedAt=DateTime.Now.AddMinutes(2), IsActive=true },
            new User(){Id=4, UserName="student1", Password=BCryptUtil.HashPassword("123456"), Email="student1@demo.com", Age=20, Access=Access.student, Gender=Gender.Male, Avatar="student1-avatar.png", CreatedAt=DateTime.Now.AddMinutes(3), IsActive=true },
            new User(){Id=5, UserName="student2", Password=BCryptUtil.HashPassword("123456"), Email="student2@demo.com", Age=21, Access=Access.student, Gender=Gender.Female, Avatar="student2-avatar.png", CreatedAt=DateTime.Now.AddMinutes(4), IsActive=true },
            new User(){Id=6, UserName="student3", Password=BCryptUtil.HashPassword("123456"), Email="student3@demo.com", Age=22, Access=Access.student, Gender=Gender.Male, Avatar="student3-avatar.png", CreatedAt=DateTime.Now.AddMinutes(5), IsActive=true }

        };

        private List<Carousel> carousels = new List<Carousel>()
        {
            new Carousel(){Id=1, Title="Welcome Banner", ImageUrl="/images/welcome.jpg", RedirectUrl="/home", IsActive=true, UpdatedAt=DateTime.Now, Position=1, StartDate=DateTime.Now.AddDays(-7), EndDate=DateTime.Now.AddDays(30), CreatedByUserId=1, UpdatedByUserId=1 },
            new Carousel(){Id=2, Title="Sale Banner", ImageUrl="/images/sale.jpg", RedirectUrl="/sale", IsActive=true, UpdatedAt=DateTime.Now, Position=2, StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddDays(10), CreatedByUserId=2, UpdatedByUserId=2 }
        };

        private List<Comment> comments = new List<Comment>()
        {
            new Comment(){Id=1, CourseId=101, CreatedByUserId=3, Content="Great course!", IsActive=true, IsFlagged=false, ParentCommentId=null, CreatedAt=DateTime.Now },
            new Comment(){Id=2, CourseId=101, CreatedByUserId=2, Content="I agree, very helpful!", IsActive=true, IsFlagged=false, ParentCommentId=1, CreatedAt=DateTime.Now.AddMinutes(1) },
            new Comment(){Id=3, CourseId=102, CreatedByUserId=3, Content="Needs more examples.", IsActive=true, IsFlagged=false, ParentCommentId=null, CreatedAt=DateTime.Now.AddMinutes(2) }
        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Users.Any())
            {
                await this._dbContext.Users.AddRangeAsync(users);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.Carousels.Any())
            {
                await this._dbContext.Carousels.AddRangeAsync(carousels);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.Comments.Any())
            {
                await this._dbContext.Comments.AddRangeAsync(comments);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}