
using Mooc.Application.Contracts.Admin.Dto.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface ICommentService
    {
        Task<CommentDto> GetCommentAsync(long id);
        Task<List<CommentDto>> GetAllCommentsAsync();
    }
}