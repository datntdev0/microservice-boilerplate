using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Common.Registars;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    internal class ServiceBootstrap<TModule> where TModule : BaseModule
    {
        private readonly IEnumerable<BaseModule> _modules = CreateAllModuleInstances();

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs) 
        { 
            _modules.ToList().ForEach(module => module.ConfigureServices(services, configs));
            _modules.ToList().ForEach(module => RegisterInjectServiceTypes(services, module.GetType().Assembly));
        }

        public void Configure(IServiceProvider serviceProvider, IConfigurationRoot configs) 
        { 
            _modules.ToList().ForEach(module => module.Configure(serviceProvider, configs));
        }

        private static IEnumerable<BaseModule> CreateAllModuleInstances()
        {
            return FindDependedModuleTypesRecursively(typeof(TModule))
                .Append(typeof(TModule))
                .Select(Activator.CreateInstance)
                .Select(module => (BaseModule)module!);
        }

        private static IEnumerable<Type> FindDependedModuleTypesRecursively(Type moduleType)
        {
            if (!moduleType.GetTypeInfo().IsDefined(typeof(DependOnAttribute), true)) return [];

            var moduleTypes = moduleType.GetTypeInfo()
                .GetCustomAttributes(typeof(DependOnAttribute), true)
                .Cast<DependOnAttribute>()
                .SelectMany(x => x.DependedModuleTypes)
                .Distinct();

            return moduleTypes
                .SelectMany(FindDependedModuleTypesRecursively)
                .Concat(moduleTypes);
        }

        private static void RegisterInjectServiceTypes(IServiceCollection services, Assembly entryAssembly)
        {
            var injectServiceTypes = entryAssembly.GetTypes()
                .Where(type => type.GetTypeInfo().CustomAttributes
                    .Any(att => att.AttributeType == typeof(InjectServiceAttribute))
                ).ToArray();

            foreach (var type in injectServiceTypes)
            {
                var lifetime = type.GetTypeInfo().GetCustomAttribute<InjectServiceAttribute>()?.Lifetime;
                if (lifetime == null) continue;

                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(type);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
