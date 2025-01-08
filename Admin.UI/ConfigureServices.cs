using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Mvc;

using NSwag;
using NSwag.Generation.Processors.Security;

namespace Admin.UI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services)
        {
            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

            services.AddHttpContextAccessor();

            //services.AddHealthChecks()
            //    .AddDbContextCheck<ApplicationDbContext>();

            //services.AddControllersWithViews(options =>
            //    options.Filters.Add<ApiExceptionFilterAttribute>())
            //        .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.AddRazorPages();            
            //services.AddSwagger();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Admin API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            return services;
        }
    }

}
