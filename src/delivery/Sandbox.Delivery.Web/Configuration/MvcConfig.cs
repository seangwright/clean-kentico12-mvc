using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection AddAppMvc(this IServiceCollection services) => services
            .AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
            })
            .AddMvcOptions(options =>
            {
                options.Conventions.Add(new FeatureConvention());
                options.CacheProfiles.Add("Default", new CacheProfile()
                {
                    Duration = 60,
                    Location = ResponseCacheLocation.Client
                });
                options.CacheProfiles.Add("Disabled", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
            })
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
                options.ViewLocationFormats.Insert(0, "/Features/{feature}/{0}.cshtml");
                options.ViewLocationFormats.Insert(0, "/Features/{1}/{0}.cshtml");
                options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            })
            .ReturnServiceCollection(services);

        private static IServiceCollection ReturnServiceCollection(this IMvcBuilder builder, IServiceCollection services) => services;
    }

    public class SharedResources { }

    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.StartsWith("Kentico"))
            {
                return;
            }

            controller.Properties.Add("feature", GetFeatureName(controller.ControllerType));
        }

        private string GetFeatureName(TypeInfo controllerType)
        {
            string[] tokens = controllerType.FullName.Split('.');

            if (!tokens.Any(t => t == "Features"))
            {
                return "";
            }

            return tokens
              .SkipWhile(t => !t.Equals("features", StringComparison.CurrentCultureIgnoreCase))
              .Skip(1)
              .Take(1)
              .FirstOrDefault();
        }
    }

    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            var controllerDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            string featureName = controllerDescriptor?.Properties["feature"] as string;

            foreach (string location in viewLocations)
            {
                yield return location.Replace("feature", featureName);
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc
        /// </summary>
        /// <param name="context"></param>
        public void PopulateValues(ViewLocationExpanderContext context) =>
            context.Values["action_displayname"] = context.ActionContext.ActionDescriptor.DisplayName;
    }
}
