﻿using datntdev.Microservices.Common.Application;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Buffers.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace datntdev.Microservices.ServiceDefaults.Providers
{
    internal partial class AppServiceControllerProvider : ControllerFeatureProvider, IApplicationModelConvention
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (typeInfo.IsClass == false || typeInfo.IsAbstract)
                return false;

            // Exclude types that are already registered as controllers
            if (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if type implements IApplicationService
            if (typeof(IApplicationService).IsAssignableFrom(typeInfo.AsType()))
                return true;

            return false;
        }

        public void Apply(ApplicationModel application)
        {
            var appServiceControllers = application.Controllers
                .Where(c => c.ControllerType.IsAssignableTo(typeof(IApplicationService)));
            foreach (var controller in appServiceControllers)
            {
                controller.ControllerName = GetConventionalControllerName(controller);
                foreach (var action in controller.Actions)
                {
                    action.Selectors.Clear();
                    action.Selectors.Add(NormalizeDefaultSelector(action));
                    action.ApiExplorer.IsVisible = true;
                }
            }
        }

        private static SelectorModel NormalizeDefaultSelector(ActionModel action)
        {
            var httpMethod = GetConventionalVerbForMethodName(action.ActionName);
            var httpMethodConstraint = new HttpMethodActionConstraint([httpMethod]);
            var routeAttribute = new AttributeRouteModel(new RouteAttribute(
                GetConventionalActionRoute(action)));

            foreach (var param in action.Parameters.Where(x => x.BindingInfo is null))
            {
                if (httpMethod == "POST" || httpMethod == "PUT")
                {
                    param.BindingInfo = BindingInfo.GetBindingInfo([new FromBodyAttribute()]);
                }
            }

            var selector = new SelectorModel() { AttributeRouteModel = routeAttribute };
            selector.ActionConstraints.Add(httpMethodConstraint);
            return selector;
        }

        private static string GetConventionalControllerName(ControllerModel controller)
        {
            return controller.ControllerName.Replace("AppService", "").Replace("ApplicationService", "");
        }

        private static string GetConventionalActionRoute(ActionModel action)
        {
            var controllerName = action.Controller.ControllerName;
            var baseRoute = $"api/{KebabCaseRegex().Replace(controllerName, "$1-$2").ToLower()}";
            if (action.ActionName == "Get") return $"{baseRoute}/{{id}}";
            if (action.ActionName == "Delete") return $"{baseRoute}/{{id}}";
            return baseRoute;
        }

        private static string GetConventionalVerbForMethodName(string actionName)
        {
            if (actionName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
                return "GET";

            if (actionName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
                return "PUT";

            if (actionName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase))
                return "DELETE";

            if (actionName.StartsWith("Create", StringComparison.OrdinalIgnoreCase))
                return "POST";

            throw new InvalidOperationException($"No conventional HTTP verb found for action '{actionName}'");
        }

        [GeneratedRegex("([a-z])([A-Z])")]
        private static partial Regex KebabCaseRegex();
    }
}
