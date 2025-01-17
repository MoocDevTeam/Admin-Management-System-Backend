using Mooc.Application.Contracts.Admin;

namespace Mooc.Application.Admin;

public class AuthenticationService : IAuthenticationService, ITransientDependency
{
    private readonly MoocDBContext _dbContext;

    public AuthenticationService(MoocDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> ValidateUserAsync(string userName, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

        return user;
    }
}
