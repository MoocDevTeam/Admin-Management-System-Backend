using System.Threading.Tasks;
using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Admin;

public interface IAuthenticationService
{
    Task<User> ValidateUserAsync(string userName, string password);
}
