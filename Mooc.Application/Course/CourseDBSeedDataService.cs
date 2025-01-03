using Microsoft.AspNetCore.Http.HttpResults;
using Mooc.Application.Admin;
using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using Mooc.Model.Entity.Course;
namespace Mooc.Application.Course
{

    [DBSeedDataOrder(2)]
    public class CourseDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        public CourseDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // If run program.cs return error and can't add data to empty database,
        // Add father table first. Example: MoocCourse has two foreign table: User, Category. Add User table -> Add Category table -> Add MoocCourse table
        private static List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin",Age=30,Email="admin@demo.com",Gender= Gender.Male, Password=BCryptUtil.HashPassword("123456"), Avatar= "some_url?"},
            new User(){Id=2, UserName="test",Age=30,Email="test@demo.com",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"), Avatar= "some_url?"},
            new User(){Id=1, UserName="admin",Age=30,Email="admin@demo.com",Gender= Gender.Male, Password=BCryptUtil.HashPassword("123456"), Avatar= "some_url?"},
            new User(){Id=2, UserName="test",Age=30,Email="test@demo.com",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"), Avatar= "some_url?"},
        };
        private static List<Category> categories = new List<Category>()
        {
            new Category(){Id=1, CategoryName="Category1",Description="",IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=2, CategoryName="Category2",Description="", ParentId=1,IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=1, CategoryName="Category1",Description="",IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=2, CategoryName="Category2",Description="", ParentId=1,IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
        };

        private static List<MoocCourse> courses = new List<MoocCourse>()
        {
            new MoocCourse() { Id = 1, Title = "React", CourseCode = "100", CoverImage = "xxx.png", Description = "React", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 2, Title = ".Net", CourseCode = "101", CoverImage = "xxx.png", Description = ".Net", CreatedByUserId = 1, UpdatedByUserId = 1, CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 3, Title = "Nodejs", CourseCode = "102", CoverImage = "xxx.png", Description = "Nodejs", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 1, Title = "React", CourseCode = "100", CoverImage = "xxx.png", Description = "React", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 2, Title = ".Net", CourseCode = "101", CoverImage = "xxx.png", Description = ".Net", CreatedByUserId = 1, UpdatedByUserId = 1, CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 3, Title = "Nodejs", CourseCode = "102", CoverImage = "xxx.png", Description = "Nodejs", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
        };



        private List<CourseInstance> courseInstances = new List<CourseInstance>()
        {
            new CourseInstance(){Id=1, MoocCourseId=1, TotalSessions=10, Status=CourseInstanceStatus.Open, Permission=CourseInstancePermission.Private, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(1), CreatedByUserId=1 ,UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=2, MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=1, MoocCourseId=1, TotalSessions=10, Status=CourseInstanceStatus.Open, Permission=CourseInstancePermission.Private, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(1), CreatedByUserId=1 ,UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=2, MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
        };

        private List<Enrollment> enrollments = new List<Enrollment>()
        {
            new Enrollment(){Id=1, CourseInstanceId=1, MaxStudents=200, EnrollmentStatus=EnrollmentStatus.Open,  EnrollStartDate=DateTime.Now, EnrollEndDate =DateTime.Now.AddMonths(1), CreatedByUserId =1 ,UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new Enrollment(){Id=2, CourseInstanceId=2, MaxStudents=50,  EnrollmentStatus=EnrollmentStatus.Close, EnrollStartDate=DateTime.Now, EnrollEndDate =DateTime.Now.AddMonths(2), CreatedByUserId =2, UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
        };

        //Teacher Data seeding
        private static List<Teacher> teachers = new List<Teacher>()
            {
            new Teacher()
            {
                Id = 1,
                Title = "Professor",
                Department = "Finance",
                Introduction = "Focus area Management accounting",
                Expertise = "Accounting and Financial Planning",
                HiredDate = DateTime.Now,
                Office = "Room211",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                UserId=2,
                IsActive = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Teacher()
            {
                Id = 2,
                Title = "Doctor",
                Department = "Computer Science",
                Introduction = "Expertise in Artificial Intelligence",
                Expertise = "AI and Machine Learning",
                HiredDate = DateTime.Now.AddYears(-2),
                Office = "Room302",
                CreatedByUserId = 2,
                UpdatedByUserId = 1,
                UserId=2,
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-2),
                UpdatedAt = DateTime.Now.AddMonths(-3)
            },
            new Teacher()
            {
                Id = 3,
                Title = "Tutor",
                Department = "Law",
                Introduction = "Specializes in Corporate Law",
                Expertise = "Corporate and Business Law",
                HiredDate = DateTime.Now.AddMonths(-8),
                Office = "Room105",
                CreatedByUserId = 1,
                UpdatedByUserId = 2,
                UserId=1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddMonths(-8),
                UpdatedAt = DateTime.Now.AddMonths(-2)
            },
            new Teacher()
            {
                Id = 4,
                Title = "Lecturer",
                Department = "Mathematics",
                Introduction = "Focus on Applied Mathematics and Statistics",
                Expertise = "Applied Mathematics",
                HiredDate = DateTime.Now.AddYears(-1),
                Office = "Room402",
                CreatedByUserId = 3,
                UpdatedByUserId = 3,
                UserId=1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-1),
                UpdatedAt = DateTime.Now
            },
            new Teacher()
            {
                Id = 5,
                Title = "Professor",
                Department = "Physics",
                Introduction = "Specialized in Quantum Mechanics",
                Expertise = "Quantum Computing and Physics",
                HiredDate = DateTime.Now.AddYears(-10),
                Office = "Room203",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                UserId=1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-10),
                UpdatedAt = DateTime.Now.AddMonths(-1)
            },
            new Teacher()
            {
                Id = 6,
                Title = "Tutor",
                Department = "Chemistry",
                Introduction = "Expert in Organic Chemistry",
                Expertise = "Organic and Inorganic Chemistry",
                HiredDate = DateTime.Now.AddYears(-3),
                Office = "Room310",
                CreatedByUserId = 2,
                UpdatedByUserId = 3,
                UserId=1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-3),
                UpdatedAt = DateTime.Now.AddMonths(-4)
            },
            new Teacher()
            {
                Id = 7,
                Title = "Doctor",
                Department = "Biology",
                Introduction = "Researcher in Genetics",
                Expertise = "Genetics and Molecular Biology",
                HiredDate = DateTime.Now.AddYears(-5),
                Office = "Room101",
                CreatedByUserId = 3,
                UpdatedByUserId = 2,
                UserId=1,
                IsActive = false,
                CreatedAt = DateTime.Now.AddYears(-5),
                UpdatedAt = DateTime.Now.AddMonths(-2)
            },
            new Teacher()
            {
                Id = 8,
                Title = "Lecturer",
                Department = "History",
                Introduction = "Focus on Modern European History",
                Expertise = "European History and Archaeology",
                HiredDate = DateTime.Now.AddMonths(-6),
                Office = "Room508",
                CreatedByUserId = 2,
                UpdatedByUserId = 3,
                UserId=2,
                IsActive = true,
                CreatedAt = DateTime.Now.AddMonths(-6),
                UpdatedAt = DateTime.Now.AddDays(-15)
            },
            new Teacher()
            {
                Id = 9,
                Title = "Professor",
                Department = "Economics",
                Introduction = "Expert in Microeconomic Theory",
                Expertise = "Microeconomics and Econometrics",
                HiredDate = DateTime.Now.AddYears(-8),
                Office = "Room407",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                UserId=2,
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-8),
                UpdatedAt = DateTime.Now
            },
            new Teacher()
            {
                Id = 10,
                Title = "Doctor",
                Department = "Philosophy",
                Introduction = "Research on Ethics and Metaphysics",
                Expertise = "Ethics and Theoretical Philosophy",
                HiredDate = DateTime.Now.AddMonths(-12),
                Office = "Room221",
                CreatedByUserId = 2,
                UpdatedByUserId = 3,
                UserId=1,
                IsActive = false,
                CreatedAt = DateTime.Now.AddMonths(-12),
                UpdatedAt = DateTime.Now.AddDays(-10)
            }
        };


        public async Task<bool> InitAsync()
        {


            if (!_dbContext.Category.Any())
            {
                await this._dbContext.Category.AddRangeAsync(categories);
                await this._dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Teachers.Any())
            {
                await this._dbContext.Teachers.AddRangeAsync(teachers);

                await this._dbContext.SaveChangesAsync();
            }
            if (!_dbContext.Enrollment.Any())
            {
                await this._dbContext.Enrollment.AddRangeAsync(enrollments);
                await this._dbContext.SaveChangesAsync();
            }
            if (!_dbContext.MoocCourses.Any())
            {
                await this._dbContext.MoocCourses.AddRangeAsync(courses);
                await this._dbContext.SaveChangesAsync();
            }
            if (!_dbContext.CourseInstances.Any())
            {
                await this._dbContext.CourseInstances.AddRangeAsync(courseInstances);
                await this._dbContext.SaveChangesAsync();
            }


            return true;
        }
    }
}
