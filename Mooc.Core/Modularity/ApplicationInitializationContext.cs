namespace Mooc.Core.Modularity;

public class ApplicationInitializationContext 
{
    public IServiceProvider ServiceProvider { get; set; }

    public ApplicationInitializationContext(IServiceProvider serviceProvider)
    {

        ServiceProvider = serviceProvider;
    }
}
