using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Admin.Application.Auth;
using Admin.Application.Interfaces.Services;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Services;
using Admin.Application.Services.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.UoW;
using Admin.Infra.Context.Connection;
using Admin.Infra.Repositories.Authorization;

namespace Admin.CrossCutting.Ioc
{
    /// <summary>
    /// DI declarations.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddTransient<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IJwtUtils, JwtUtils>();

            //Application Services
            services
                .AddScoped<IAuthAppService, AuthAppService>()
                .AddScoped<ISubscriberAppService, SubscriberAppService>()                
                .AddScoped<ITransactionAppService, TransactionAppService>()
                .AddScoped<IProfileAppService, ProfileAppService>()
                .AddScoped<IUserAppService, UserAppService>()
                .AddScoped<IUserPreferencesAppService, UserPreferencesAppService>();

            //Repository
            services
                .AddScoped<ISubscriberRepository, SubscriberRepository>()                
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<ITransactionActionsRepository, TransactionActionsRepository>()
                .AddScoped<IProfileRepository, ProfileRepository>()
                .AddScoped<IProfileTransactionsRepository, ProfileTransactionsRepository>()
                .AddScoped<IProfileActionsRepository, ProfileActionsRepository>()
                .AddScoped<IMenuRepository, MenuRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserPreferencesRepository, UserPreferencesRepository>()
                .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();            

            //Repository UnitOfWork
            services
                .AddScoped<UnitOfWorkRepository>()
                .AddScoped<IUnitOfWorkRepository>(x => x.GetRequiredService<UnitOfWorkRepository>())
                .AddScoped<IUnitOfWork>(x => x.GetRequiredService<UnitOfWorkRepository>());            

            return services;
        }
    }
}
