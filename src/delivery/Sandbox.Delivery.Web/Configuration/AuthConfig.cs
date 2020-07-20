using System;
using System.Threading.Tasks;
using CMS.Helpers;
using Kentico.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class AuthConfig
    {
        // Application authentication cookie name
        public const string AUTHENTICATION_COOKIE_NAME = "identity.authentication";

        /// <summary>
        /// Adds authentication related services and configuration to the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppAuth(this IServiceCollection services, string defaultCutlure)
        {
            services.AddScoped<IPasswordHasher<ApplicationUser>, Kentico.Membership.PasswordHasher<ApplicationUser>>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Note: These settings are effective only when password policies are turned off in the administration settings.
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddApplicationDefaultTokenProviders()
            .AddUserStore<ApplicationUserStore<ApplicationUser>>()
            .AddRoleStore<ApplicationRoleStore<ApplicationRole>>()
            .AddUserManager<ApplicationUserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddAuthorization();
            services.AddAuthentication();

            services.ConfigureApplicationCookie(c =>
            {
                c.Events.OnRedirectToLogin = ctx =>
                {
                    string culture = (string)ctx.Request.RouteValues["culture"];

                    if (string.IsNullOrEmpty(culture))
                    {
                        culture = defaultCutlure;
                    }

                    string redirectUrl = ctx.RedirectUri.Replace("/Account/Login", $"/{culture}/Account/Login");
                    ctx.Response.Redirect(redirectUrl);
                    return Task.CompletedTask;
                };
                c.LoginPath = new PathString("/Account/Login");
                c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.SlidingExpiration = true;
                c.Cookie.Name = AUTHENTICATION_COOKIE_NAME;
            });

            CookieHelper.RegisterCookie(AUTHENTICATION_COOKIE_NAME, CookieLevel.Essential);

            return services;
        }
    }
}
