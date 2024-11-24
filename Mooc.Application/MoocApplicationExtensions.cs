using Microsoft.Extensions.DependencyInjection;

namespace Mooc.Application;

public static class MoocApplicationExtensions
{
    public  static void AddApplication(this IServiceCollection services)
    {
        //services.AddTransient<IPermissionChecker, PermissionChecker>();
        //services.AddTransient<IDBSeedDataService, AdminDBSeedDataService>();
        //services.AddTransient<IDBSeedDataService, MoocDBSeedDataService>();
        //services.AddTransient<IUserService, UserService>();
        //services.AddTransient<ICategoryService, CategoryService>();

        //Add automapper
        services.AddAutoMapper(typeof(MoocApplicationExtensions));
    }
}
