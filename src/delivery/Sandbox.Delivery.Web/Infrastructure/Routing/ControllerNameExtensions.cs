using System;
using System.Web.Mvc;
using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Infrastructure.Routing
{
    public static class ControllerNameExtensions
    {
        /**
         * "Controller".Length
         */
        private const int CONTROLLER_SUFFIX_LENGTH = 10;

        public static string RemoveControllerSuffix(this Type controllerType)
        {
            Guard.Against.Null(controllerType, nameof(controllerType));

            if (!typeof(Controller).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException($"Type [{controllerType.Name}] is not assignable from [{nameof(Controller)}]");
            }

            return controllerType
                .Name
                .Substring(0, controllerType.Name.Length - CONTROLLER_SUFFIX_LENGTH);
        }
    }
}
