using Autofac;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace Mooc.Application;

public class AutofacModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        //builder.RegisterType<PermissionChecker>().As<IPermissionChecker>().InstancePerDependency();
        //builder.RegisterType<AdminDBSeedDataService>().As<IDBSeedDataService>().InstancePerDependency();
        //builder.RegisterType<MoocDBSeedDataService>().As<IDBSeedDataService>().InstancePerDependency();
        //builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
        //builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerDependency();

        //var assemblys = Assembly.GetExecutingAssembly();
        //builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<ITransientDependency>()).
        //    AsImplementedInterfaces().
        //    InstancePerDependency();

        //builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<IScopedDependency>()).
        //   AsImplementedInterfaces().
        //   InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<ISingletonDependency>()).
        //   AsImplementedInterfaces().
        //   SingleInstance();


        var compilationLibrary = DependencyContext.Default.RuntimeLibraries.Where(x => x.Name.StartsWith("Mooc."));
        List<Assembly> assemblyList = new List<Assembly>();
        foreach (var iLibrary in compilationLibrary)
        {
            try
            {
                assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(iLibrary.Name)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(iLibrary.Name + ex.Message);
            }
        }
        var assemblys = assemblyList.ToArray();
        builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<ITransientDependency>()).
            AsImplementedInterfaces().
            InstancePerDependency();

        builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<IScopedDependency>()).
           AsImplementedInterfaces().
           InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assemblys).Where(x => x.IsAssignableTo<ISingletonDependency>()).
           AsImplementedInterfaces().
           SingleInstance();
    }
}
