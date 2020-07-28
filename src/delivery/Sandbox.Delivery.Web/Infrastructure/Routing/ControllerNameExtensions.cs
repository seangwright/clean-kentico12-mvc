using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;

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

            if (!typeof(ControllerBase).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException($"Type [{controllerType.Name}] is not assignable from [{nameof(Controller)}]");
            }

            return controllerType
                .Name
                .Substring(0, controllerType.Name.Length - CONTROLLER_SUFFIX_LENGTH);
        }
    }
}
