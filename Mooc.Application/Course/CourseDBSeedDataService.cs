using Microsoft.AspNetCore.Http.HttpResults;
using Mooc.Application.Admin;
using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using Mooc.Model.Entity;
using Mooc.Shared.Enum;
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
        private static List<Category> categories = new List<Category>()
        {
            new Category(){Id=1, CategoryName="Programming Language",Description="",IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=2, CategoryName="Business and Accounting",Description="", ParentId=1,IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=3, CategoryName="Social",Description="", ParentId=1,IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Category(){Id=4, CategoryName="Software Engineering",Description="", ParentId=1,IconUrl="xxx@demo.com",CreatedByUserId=1,CreatedAt= DateTime.Now },
        };

        private static List<MoocCourse> courses = new List<MoocCourse>()
        {
            new MoocCourse() { Id = 1, Title = "React.js", CourseCode = "100", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/a/a7/React-icon.svg", Description = "For Frontend Developer", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 2, Title = ".Net", CourseCode = "101", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/7/7d/Microsoft_.NET_logo.svg", Description = "For Backend Developer", CreatedByUserId = 1, UpdatedByUserId = 1, CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 3, Title = "Advanced Node.js", CourseCode = "102", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/d/d9/Node.js_logo.svg", Description = "Everything can be build in js will be build in js", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 4, Title = "Java", CourseCode = "103", CoverImage = "https://upload.wikimedia.org/wikipedia/en/3/30/Java_programming_language_logo.svg", Description = "Java", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 5, Title = "C++", CourseCode = "104", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/1/18/ISO_C%2B%2B_Logo.svg", Description = "C++", CreatedByUserId = 1, UpdatedByUserId = 2, CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 6, Title = "Python", CourseCode = "105", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/c/c3/Python-logo-notext.svg", Description = "Python", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 7, Title = "Financial Statement", CourseCode = "106", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/3/34/Microsoft_Office_Excel_%282019%E2%80%93present%29.svg", Description = "Business Analysis", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 8, Title = "Cost Management", CourseCode = "107", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/3/34/Microsoft_Office_Excel_%282019%E2%80%93present%29.svg", Description = "For Senior Accounting", CreatedByUserId = 1, UpdatedByUserId = 2, CategoryId=2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 9, Title = "Teamworking Principles", CourseCode = "108", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/6/68/Monts-merveilles.fr_teambuilding.jpg", Description = "Soft Skills", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=3, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },
            new MoocCourse() { Id = 10, Title = "Communication SKills at Work", CourseCode = "109", CoverImage = "https://upload.wikimedia.org/wikipedia/commons/1/13/Framework_for_21st_Century_Learning.svg", Description = "Soft Skills", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=3, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now.AddMinutes(1) },

        };

        private List<CourseInstance> courseInstances = new List<CourseInstance>()
        {
            new CourseInstance(){Id=1, Description="Publish-2023-01", MoocCourseId=1, TotalSessions=10, Status=CourseInstanceStatus.Open, Permission=CourseInstancePermission.Private, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(1), CreatedByUserId=1 ,UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=2, Description="Publish-2023-03", MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=3, Description="Publish-2023-05", MoocCourseId=1, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=4, Description="Publish-2023-08", MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=5, Description="Publish-2024-01", MoocCourseId=1, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=6, Description="Publish-2024-02", MoocCourseId=1, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=7, Description="Publish-2024-06", MoocCourseId=1, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
            new CourseInstance(){Id=8, Description="Publish-2024-06", MoocCourseId=2, TotalSessions=20, Status=CourseInstanceStatus.Close, Permission=CourseInstancePermission.Public, StartDate=DateTime.Now, EndDate=DateTime.Now.AddMonths(2), CreatedByUserId=1 ,UpdatedByUserId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
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

        //TeacherCourseInstance Data seeding
        private static List<TeacherCourseInstance> teacherCourseInstances = new List<TeacherCourseInstance>()
        {
            new TeacherCourseInstance() { Id = 1, PermissionType = TeacherCourseInstancePermissionType.CanEdit, CourseInstanceId = 1, TeacherId = 1, CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now, CreatedByUserId= 1, UpdatedByUserId= 1 },
            new TeacherCourseInstance() { Id = 2, PermissionType = TeacherCourseInstancePermissionType.NoAutherization, CourseInstanceId = 1, TeacherId = 2, CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now,CreatedByUserId= 1, UpdatedByUserId= 1}
        };

        //Session Data seeding
        private List<Session> sessions = new List<Session>()
        {
            new Session(){
                Id=1,
                Title = "Introduction to React",
                Description = "Overview of the React framework",
                Order = 1,
                CourseInstanceId = 1, // relate to CourseInstanceId 1
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1)
                },
            new Session(){
                Id=2,
                Title = "Setting up the Development Environment",
                Description = "Step-by-step guide",
                Order = 2,
                CourseInstanceId = 1, // relate to CourseInstanceId 1
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1)
                },
            new Session(){
                Id=3,
                Title = "Basic Syntax and Data Types",
                Description = "Learn about variables",
                Order = 3,
                CourseInstanceId = 1, // relate to CourseInstanceId 1
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1)
                },
            new Session(){
                Id=4,
                Title = "Thinking in React",
                Description = "Thinking in React",
                Order = 4,
                CourseInstanceId = 1, // relate to CourseInstanceId 1
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1)
                },
            new Session(){
                Id=5,
                Title = "Advanced C# Features",
                Description = "Learn about delegates, events, and lambda expressions",
                Order = 5,
                CourseInstanceId = 2, // relate to CourseInstanceId 2
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1),
                },
            new Session(){
                Id=6,
                Title = "Async Programming in C#",
                Description = "Introduction to async and await",
                Order = 6,
                CourseInstanceId = 2, // relate to CourseInstanceId 2
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1),
            },
            new Session(){
                Id=7,
                Title = "Testing C# Applications",
                Description = "Learn how to effectively test and debug C# code",
                Order = 7,
                CourseInstanceId = 2, // relate to CourseInstanceId 2
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now.AddMinutes(1),
            }
        };

        //Media Data seeding
        private List<Media> mediaData = new List<Media>()
        {
            // SessionId=1,have two Video files
            new Media()
            {
                Id = 1,
                SessionId = 1,
                FileType = MediaFileType.Video,
                FileName = "Introduction_to_.Net_video1.mp4",
                FilePath = "/files/sessions/1/Introduction_to_.Net_video1.mp4",
                ThumbnailPath = "/thumbnails/sessions/1/Introduction_to_.Net_video1.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Approved
            },
            new Media()
            {
                Id = 2,
                SessionId = 1,
                FileType = MediaFileType.Video,
                FileName = "Introduction_to_.Net_video2.mp4",
                FilePath = "/files/sessions/1/Introduction_to_.Net_video2.mp4",
                ThumbnailPath = "/thumbnails/sessions/1/Introduction_to_.Net_video2.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Pending
            },

            // SessionId=2,have two pdf files
            new Media()
            {
                Id = 3,
                SessionId = 2,
                FileType = MediaFileType.Pdf,
                FileName = "Setting_up_the_Development_Environment_doc1.pdf",
                FilePath = "/files/sessions/2/Setting_up_the_Development_Environment_doc1.pdf",
                ThumbnailPath = "/thumbnails/sessions/2/Setting_up_the_Development_Environment_doc1.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Approved
            },
            new Media()
            {
                Id = 4,
                SessionId = 2,
                FileType = MediaFileType.Pdf,
                FileName = "Setting_up_the_Development_Environment_doc2.pdf",
                FilePath = "/files/sessions/2/Setting_up_the_Development_Environment_doc2.pdf",
                ThumbnailPath = "/thumbnails/sessions/2/Setting_up_the_Development_Environment_doc2.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Pending
            },

            // SessionId=3, none meida files

            // SessionId=4, one vedio and one pdf file
            new Media()
            {
                Id = 5,
                SessionId = 4,
                FileType = MediaFileType.Video,
                FileName = "Basic_Syntax_and_Data_Types_video.mp4",
                FilePath = "/files/sessions/4/Basic_Syntax_and_Data_Types_video.mp4",
                ThumbnailPath = "/thumbnails/sessions/4/Basic_Syntax_and_Data_Types_video.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Pending
            },
            new Media()
            {
                Id = 6,
                SessionId = 4,
                FileType = MediaFileType.Pdf,
                FileName = "Basic_Syntax_and_Data_Types_doc.pdf",
                FilePath = "/files/sessions/4/Basic_Syntax_and_Data_Types_doc.pdf",
                ThumbnailPath = "/thumbnails/sessions/4/Basic_Syntax_and_Data_Types_doc.png",
                CreatedByUserId = 1,
                UpdatedByUserId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now.AddMinutes(1),
                ApprovalStatus = MediaApprovalStatus.Rejected
            }
        };


        public async Task<bool> InitAsync()
        {
            if (!_dbContext.Category.Any())
            {
                await this._dbContext.Category.AddRangeAsync(categories);
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

            if (!_dbContext.Enrollment.Any())
            {
                await this._dbContext.Enrollment.AddRangeAsync(enrollments);
                await this._dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Session.Any())
            {
                await this._dbContext.Session.AddRangeAsync(sessions);
                await this._dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Teachers.Any())
            {
                await this._dbContext.Teachers.AddRangeAsync(teachers);
                await this._dbContext.SaveChangesAsync();
            }

            if (!_dbContext.TeacherCourseInstances.Any())
            {
                await this._dbContext.TeacherCourseInstances.AddRangeAsync(teacherCourseInstances);
                await this._dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Media.Any())
            {
                await this._dbContext.Media.AddRangeAsync(mediaData);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}

