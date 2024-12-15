using Mooc.Application.Contracts;
using Mooc.Core.Utils;
namespace Mooc.Application.Course
{
    public class CourseDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        public CourseDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        //private List<MoocCourseInstance> moocCourseInstances= new List<MoocCourseInstance>()
        //{
        //    new MoocCourseInstance(){Id=1, SesstionId=1, CourseId=1, TeacherId=1, StartDate="1", EndDate="1", TotalSession=1, OpenStatus=MoocCourseInstanceOpenStatus.Open, Permisstion = MoocCourseInstancePermission.Private, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now  },
        //};

        private List<MoocCourse> courses = new List<MoocCourse>()
        {
            new MoocCourse(){Id=1, Title="React",CourseCode="100",CoverImage="xxx.png",Description="React",CreatedByUserId= 1,UpdatedByUserId=1, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=2, Title=".Net",CourseCode="101",CoverImage="xxx.png",Description=".Net",CreatedByUserId= 1,UpdatedByUserId=1, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=3, Title="Nodejs",CourseCode="102",CoverImage="xxx.png",Description="Nodejs",CreatedByUserId= 1,UpdatedByUserId=1, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=4, Title="Python",CourseCode="103",CoverImage="xxx.png",Description="Python",CreatedByUserId= 1,UpdatedByUserId=1, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=5, Title="Java",CourseCode="104",CoverImage="xxx.png",Description="Java",CreatedByUserId= 1,UpdatedByUserId=1, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=6, Title="Nextjs",CourseCode="105",CoverImage="xxx.png",Description="Nextjs",CreatedByUserId= 2,UpdatedByUserId=2, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=7, Title="C++",CourseCode="106",CoverImage="xxx.png",Description="C++",CreatedByUserId= 2,UpdatedByUserId=2, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=8, Title="AI",CourseCode="107",CoverImage="xxx.png",Description="AI",CreatedByUserId= 2,UpdatedByUserId=2, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},
            new MoocCourse(){Id=9, Title="AWS",CourseCode="108",CoverImage="xxx.png",Description="AWS",CreatedByUserId= 2,UpdatedByUserId=2, CreatedAt=DateTime.Now ,UpdatedAt=DateTime.Now.AddMinutes(1)},

        };
        public async Task<bool> InitAsync()
        {
            //空白数据库添加文件
            //if (!this._dbContext.MoocCourseInstances.Any())
            //{

            //    await this._dbContext.MoocCourseInstances.AddRangeAsync(moocCourseInstances);
            //    await this._dbContext.SaveChangesAsync();
            //}


            //数据库已有数据,更新数据
            foreach (var course in courses)
            {
                var existingCourse = await this._dbContext.MoocCourses
                    .FirstOrDefaultAsync(c => c.Id == course.Id); // 根据唯一标识符（例如 CourseId）查找

                if (existingCourse != null)
                {
                    // 更新已有记录
                    existingCourse.Title = course.Title; // 示例字段更新
                    existingCourse.Description = course.Description;
                    existingCourse.CourseCode = course.CourseCode;
                    existingCourse.CoverImage = course.CoverImage;
                    existingCourse.UpdatedAt = DateTime.UtcNow; // 示例更新字段
                }
                else
                {
                    // 添加新记录
                    await this._dbContext.MoocCourses.AddAsync(course);
                }
            }

            // 保存所有更改
            await this._dbContext.SaveChangesAsync();

            return true;
        }
    }
}