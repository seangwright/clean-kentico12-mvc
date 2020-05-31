using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.Routing
{
    public interface IControllerActionMatchProvider
    {
        bool TryFindMatch(string nodeClassName, out ControllerActionPair match);
    }

    public class ControllerActionMatchProvider : IControllerActionMatchProvider
    {
        private readonly Dictionary<string, ControllerActionPair> classNameLookup = new Dictionary<string, ControllerActionPair>();

        public ControllerActionMatchProvider(IEnumerable<Type> assemblySourceTypes)
        {
            var controllerTypes = assemblySourceTypes
                .Where(t =>
                    t.IsPublic &&
                    t != typeof(Controller) &&
                    typeof(Controller).IsAssignableFrom(t));

            foreach (var controllerType in controllerTypes)
            {
                var actions = controllerType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.IsDefined(typeof(PageTypeRouteAttribute)));

                foreach (var action in actions)
                {
                    string[] classNames = action.GetCustomAttribute<PageTypeRouteAttribute>().ClassNames;

                    foreach (string className in classNames)
                    {
                        if (classNameLookup.ContainsKey(className))
                        {
                            var pair = classNameLookup[className];

                            throw new Exception(
                                "Duplicate Annotation: " +
                                $"{pair.ControllerName}Controller.{pair.ActionName} already registered for NodeClassName {className}. " +
                                $"Cannot be registered for {controllerType.Name}.{action.Name}"
                            );
                        }

                        classNameLookup.Add(className, new ControllerActionPair(controllerType.RemoveControllerSuffix(), action.Name));
                    }
                }
            }
        }

        public bool TryFindMatch(string nodeClassName, out ControllerActionPair match) =>
            classNameLookup.TryGetValue(nodeClassName, out match);
    }

    public readonly struct ControllerActionPair
    {
        public string ControllerName { get; }
        public string ActionName { get; }

        public ControllerActionPair(string controllerName, string actionName)
        {
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}
