using Microsoft.Extensions.DependencyInjection;

namespace Mooc.Application;

public static class MoocApplicationExtensions
{
    public  static void AddApplication(this IServiceCollection services)
    {
        //Add automapper
        services.AddAutoMapper(typeof(MoocApplicationExtensions));
    }
}
