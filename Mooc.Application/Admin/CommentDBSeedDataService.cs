using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using Mooc.Shared.Enum;

namespace Mooc.Application.Admin
{
    [DBSeedDataOrder(9)]
    public class CommentDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public CommentDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<Comment> comments = new List<Comment>()
        {
            new Comment(){Id=1, MoocCourseId=1, CategoryId=1, SessionId=1, TeacherId=1, CreatedByUserId=1, Content="This session is really informative and well-explained!", IsActive=true, IsFlagged=false, ParentCommentId=null, CreatedAt=DateTime.Now, UpdatedAt=null},
            new Comment(){Id=2, MoocCourseId=1, CategoryId=1, SessionId=1, TeacherId=1, CreatedByUserId=2, Content="Totally agree! The examples were very helpful.", IsActive=true, IsFlagged=false, ParentCommentId=1, CreatedAt=DateTime.Now.AddMinutes(1), UpdatedAt=null}
        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Comments.Any())
            {
                await this._dbContext.Comments.AddRangeAsync(comments);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
