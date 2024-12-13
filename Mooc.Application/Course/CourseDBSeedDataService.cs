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

        public async Task<bool> InitAsync()
        {

            //if (!this._dbContext.MoocCourseInstances.Any())
            //{

            //    await this._dbContext.MoocCourseInstances.AddRangeAsync(moocCourseInstances);
            //    await this._dbContext.SaveChangesAsync();
            //}

            return true;
        }
    }
}
