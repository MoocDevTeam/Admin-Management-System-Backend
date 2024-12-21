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

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Users.Any())
            {
                await this._dbContext.Users.AddRangeAsync(users);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}