using System.Reflection;
using DelightBistroMvc.Data.Repositories;
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.RelfectionTools;

public static class AutoResolver
{
    public static void ResolveRepositories(this IServiceCollection serviceCollection)
    {
        var baseReposytoryType = typeof(BaseRepository<>);
        var iBaseRepositoryType = typeof(IBaseRepository<>);
        var assembly = Assembly.GetAssembly(baseReposytoryType);

        var repositoryInterfaces = assembly
            .GetTypes()
            .Where(x => x.IsInterface
                && !x.IsGenericType
                && x.GetInterfaces().Any(parentInterface =>
                    parentInterface.IsGenericType
                    && parentInterface.GetGenericTypeDefinition() == iBaseRepositoryType))
            .ToList();

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var repositoryClass = assembly
                .GetTypes()
                .FirstOrDefault(x => x.IsClass
                    && x.GetInterfaces()
                        .Any(classInterfaceType => classInterfaceType == repositoryInterface));

            var constructor = repositoryClass
                .GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .First();
            RegisterTypeByConstructor(serviceCollection, repositoryInterface, constructor);
        }
    }

    public static void ResolveByAttribute(this IServiceCollection serviceCollection)
    {
        var attributeType = typeof(AutoRegisterAttribute);
        var assembly = Assembly.GetAssembly(attributeType);
        RegisterByAttributeOnConstructor(serviceCollection, assembly);
        RegisterByAttributeOnClass(serviceCollection, assembly);
    }

    private static void RegisterByAttributeOnConstructor(IServiceCollection serviceCollection, Assembly? assembly)
    {
        var classesForAuthRegister = assembly.GetTypes()
                        .Where(x => x.IsClass
                            && x.GetConstructors()
                            .Any(c => c.GetCustomAttribute<AutoRegisterAttribute>() != null));

        foreach (var classeForAuthRegisterin in classesForAuthRegister)
        {
            var constructors = classeForAuthRegisterin
                .GetConstructors()
                .First(x => x.GetCustomAttribute<AutoRegisterAttribute>() != null);
            var typeForeRegister = CalculateTypeForResolve(assembly, classeForAuthRegisterin);
            RegisterTypeByConstructor(serviceCollection, typeForeRegister, constructors);
        }
    }

    private static void RegisterByAttributeOnClass(IServiceCollection serviceCollection, Assembly? assembly)
    {
        var classesForAuthRegister = assembly.GetTypes()
                        .Where(x => x.IsClass
                            && x.GetCustomAttribute<AutoRegisterAttribute>() != null);

        foreach (var classeForAuthRegisterin in classesForAuthRegister)
        {
            var constructors = classeForAuthRegisterin
                .GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .First();
            var typeForeRegister = CalculateTypeForResolve(assembly, classeForAuthRegisterin);
            RegisterTypeByConstructor(serviceCollection, typeForeRegister, constructors);
        }
    }

    private static Type CalculateTypeForResolve(Assembly assembly, Type classeForAuthRegisterin)
    {
        var serviceName = classeForAuthRegisterin.Name;
        var insterfaceForService = assembly
            .GetTypes()
            .FirstOrDefault(x => x.IsInterface
                && x.Name == $"I{serviceName}");

        return insterfaceForService ?? classeForAuthRegisterin;
    }

    private static void RegisterTypeByConstructor(IServiceCollection serviceCollection, Type repositoryInterface, ConstructorInfo constructor)
    {
        var parameters = constructor.GetParameters();

        serviceCollection.AddScoped(repositoryInterface, serviceProvider =>
        {
            var paramObjects = parameters
                .Select(p => serviceProvider.GetService(p.ParameterType))
                .ToArray();

            var obj = constructor.Invoke(paramObjects);

            return obj;
        });
    }
}
