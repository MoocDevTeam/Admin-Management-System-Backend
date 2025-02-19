using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mooc.Application.Contracts.Admin
{
    public interface IAvatarService
    {
        public Task<string> UploadAvatarAsync(string userName, IFormFile file);
        public Task DeleteAvatarAsync(string userName);
        public Task<string> GetAvatarUrlAsync(string userName);
    }

}
