using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AngularMarketplace.Server.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStored(this IServiceCollection services)
        {
            services
                .AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequireNonAlphanumeric = false;

            });
            return services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme =
                    x.DefaultChallengeScheme =
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(y =>
                    {
                    y.SaveToken = false;
                    y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:JWTSecret"]!))
                    };
                });
            return services;
        }

        public static WebApplication AddIdentityAuthMiddlwares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
