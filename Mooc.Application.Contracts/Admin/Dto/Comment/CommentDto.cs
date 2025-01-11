
using System;

namespace Mooc.Application.Contracts.Admin.Dto.Comment
{
    public class CommentDto
    {
        public long Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
