using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

namespace AspNetCoreExample.Generator
{
    public class ApiControllerTypeBuildingContext : ApiControllerTypeBuildingContextBase
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, Type type)
            : base(unit, type)
        {
        }

        public static bool Accept(Type type)
        {
            return typeof(ControllerBase).IsAssignableFrom(type);
        }

        protected override TypeLocation GetApiBase(Type controllerType)
        {
            var apiBaseName = GetApiBaseName(controllerType);
            return new TypeLocation
                {
                    Name = apiBaseName,
                    Location = $"apiBase/{apiBaseName}",
                };
        }

        private string GetApiBaseName(Type controllerType)
        {
            if (IsUserScopedApi(controllerType))
                return "UserApiBase";
            return "ApiBase";
        }

        protected override Type ResolveReturnType(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>) || genericTypeDefinition == typeof(ActionResult<>))
                    return ResolveReturnType(type.GetGenericArguments()[0]);
            }

            if (type == typeof(Task))
                return typeof(void);
            if (type == typeof(ActionResult))
                return typeof(void);
            return type;
        }

        protected override BaseApiMethod ResolveBaseApiMethod(MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttribute<HttpGetAttribute>() != null)
                return BaseApiMethod.Get;

            if (methodInfo.GetCustomAttribute<HttpPostAttribute>() != null)
                return BaseApiMethod.Post;

            if (methodInfo.GetCustomAttribute<HttpPutAttribute>() != null)
                return BaseApiMethod.Put;

            if (methodInfo.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return BaseApiMethod.Delete;

            throw new NotSupportedException($"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.DeclaringType?.Name}");
        }

        protected override string BuildRoute(Type controllerType, MethodInfo methodInfo)
        {
            var routeTemplate = methodInfo.GetCustomAttributes()
                                          .Select(x => x is IRouteTemplateProvider routeTemplateProvider ? routeTemplateProvider.Template : null)
                                          .SingleOrDefault(x => !string.IsNullOrEmpty(x));
            return AppendRoutePrefix(routeTemplate, controllerType);
        }

        protected override bool PassParameterToCall(ParameterInfo parameterInfo, Type controllerType)
        {
            if (IsUserScopedApi(controllerType) && parameterInfo.Name == "userId")
                return false;
            return true;
        }

        protected override ParameterInfo[] GetQueryParameters(ParameterInfo[] parameters, Type controllerType)
        {
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !x.GetCustomAttributes<FromBodyAttribute>().Any()).ToArray();
        }

        protected override ParameterInfo GetBody(ParameterInfo[] parameters, Type controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && x.GetCustomAttributes<FromBodyAttribute>().Any());
        }

        protected override MethodInfo[] GetMethodsToImplement(Type controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(m => !m.IsSpecialName)
                                 .Where(x => x.DeclaringType == controllerType)
                                 .ToArray();
        }

        private string AppendRoutePrefix(string routeTemplate, Type controllerType)
        {
            var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();
            var fullRoute = (routeAttribute == null ? "" : routeAttribute.Template + "/") + routeTemplate;
            if (IsUserScopedApi(controllerType))
                return fullRoute.Substring("v1/user/{userId}/".Length);
            return fullRoute.Substring("v1/".Length);
        }

        private bool IsUserScopedApi(Type controller)
        {
            var route = controller.GetCustomAttribute<RouteAttribute>();
            return route?.Template.StartsWith("v1/user/{userId}") ?? false;
        }
    }
}