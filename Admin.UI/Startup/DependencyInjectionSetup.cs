using System.Reflection;
using MediatR;
using Newtonsoft.Json.Serialization;

using Admin.CrossCutting.Ioc;

namespace Admin.UI.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                                                    options.SerializerSettings.ContractResolver =
                                                    new CamelCasePropertyNamesContractResolver());
        services.AddApplicationServices();
        services.AddInfrastructure(configuration);
        services.AddWebUIServices();
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
