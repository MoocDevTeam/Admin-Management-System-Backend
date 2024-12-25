using Microsoft.AspNetCore.Http.HttpResults;
using Mooc.Application.Admin;
using Mooc.Application.Contracts;
using Mooc.Core.Utils;
using Mooc.Model.Entity.Course;
namespace Mooc.Application.Course
{
    public class CourseDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        public CourseDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // If run program.cs return error adn can't add data to empty database,
        // Add father table first. Example: MoocCourse has two foreign table: User, Category. Add User table -> Add Category table -> Add MoocCourse table
        private static List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin",Age=30,Email="admin@demo.com",Address="Canberra",Gender= Gender.Male, Password=BCryptUtil.HashPassword("123456"), Created=DateTime.Now  },
            new User(){Id=2, UserName="test",Age=30,Email="test@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"), Created=DateTime.Now.AddMinutes(1)},
        };
        private static List<Category> categories = new List<Category>()
        {
            new Category(){Id=1, CategoryName="Category1",Description="",IconUrl="xxx@demo.com",CreatedByUser=users.First(u => u.Id == 1),CreatedAt= DateTime.Now },
            new Category(){Id=2, CategoryName="Category2",Description="",IconUrl="xxx@demo.com",CreatedByUser=users.First(u => u.Id == 1),CreatedAt= DateTime.Now },
        };

        private static List<MoocCourse> courses = new List<MoocCourse>()
        {
            new MoocCourse(){Id=1, Title="React",CourseCode="100",CoverImage="xxx.png",Description="React",CreatedByUser=users.First(u => u.Id == 1) ,UpdatedByUser=users.First(u => u.Id == 1),Category= categories.First(u => u.Id == 1),CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=2, Title=".Net",CourseCode="101",CoverImage="xxx.png",Description=".Net",CreatedByUser= users.First(u => u.Id == 1),UpdatedByUser=users.First(u => u.Id == 1), Category= categories.First(u => u.Id == 1),CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=3, Title="Nodejs",CourseCode="102",CoverImage="xxx.png",Description="Nodejs",CreatedByUser= users.First(u => u.Id == 1),UpdatedByUser=users.First(u => u.Id == 1), Category= categories.First(u => u.Id == 1),CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
        };

        private List<CourseInstance> courseInstances = new List<CourseInstance>()
        {
            new CourseInstance(){Id=1, MoocCourseId=1, TotalSessions=10, Status=CourseInstanceStatus.Open, Permission=CourseInstancePermission.Private, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(1), CreatedByUser=users.First(u => u.Id == 1) ,UpdatedByUser=users.First(u => u.Id == 1), CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=2, MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUser=users.First(u => u.Id == 1) ,UpdatedByUser=users.First(u => u.Id == 1), CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
        };

        public async Task<bool> InitAsync()
        {

            await this._dbContext.Users.AddRangeAsync(users);
            await this._dbContext.MoocCourses.AddRangeAsync(courses);
            await this._dbContext.CourseInstances.AddRangeAsync(courseInstances);
            await this._dbContext.SaveChangesAsync();
            return true;
        }
    }
}
