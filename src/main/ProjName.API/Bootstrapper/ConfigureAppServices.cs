using ProjName.Application.Shared.Behaviors;
using ProjName.Application.Shared.Interfaces;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace ProjName.API.Bootstrapper;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new List<Assembly>
        {
            Assembly.GetAssembly(typeof(Application.Interfaces.IAppDbContext))!,
            Assembly.GetAssembly(typeof(ICoreDbContext))!
        };

        services.AddAutoMapper(x =>
        {
            x.AddProfile(new MappingProfile(Assembly.GetAssembly(typeof(Application.Interfaces.IAppDbContext))));
            x.AddProfile(new MappingProfile(Assembly.GetAssembly(typeof(ICoreDbContext))));
        });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddMediatR(op => op.RegisterServicesFromAssemblies(assemblies.ToArray()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
