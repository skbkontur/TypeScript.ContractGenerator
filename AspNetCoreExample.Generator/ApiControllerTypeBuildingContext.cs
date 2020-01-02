using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

namespace AspNetCoreExample.Generator
{
    public class ApiControllerTypeBuildingContext : ApiControllerTypeBuildingContextBase
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public static bool Accept(Type type)
        {
            return typeof(ControllerBase).IsAssignableFrom(type);
        }

        protected override TypeLocation GetApiBase(ITypeInfo controllerType)
        {
            var apiBaseName = GetApiBaseName(controllerType);
            return new TypeLocation
                {
                    Name = apiBaseName,
                    Location = $"apiBase/{apiBaseName}",
                };
        }

        private string GetApiBaseName(ITypeInfo controllerType)
        {
            if (IsUserScopedApi(controllerType))
                return "UserApiBase";
            return "ApiBase";
        }

        protected override ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            var type = typeInfo.Type;
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>) || genericTypeDefinition == typeof(ActionResult<>))
                    return ResolveReturnType(new TypeWrapper(type.GetGenericArguments()[0]));
            }

            if (type == typeof(Task))
                return new TypeWrapper(typeof(void));
            if (type == typeof(ActionResult))
                return new TypeWrapper(typeof(void));
            return new TypeWrapper(type);
        }

        protected override BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo)
        {
            var method = methodInfo.Method;
            if (method.GetCustomAttribute<HttpGetAttribute>() != null)
                return BaseApiMethod.Get;

            if (method.GetCustomAttribute<HttpPostAttribute>() != null)
                return BaseApiMethod.Post;

            if (method.GetCustomAttribute<HttpPutAttribute>() != null)
                return BaseApiMethod.Put;

            if (method.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return BaseApiMethod.Delete;

            throw new NotSupportedException($"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.Method.DeclaringType?.Name}");
        }

        protected override string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo)
        {
            var routeTemplate = methodInfo.Method.GetCustomAttributes()
                                          .Select(x => x is IRouteTemplateProvider routeTemplateProvider ? routeTemplateProvider.Template : null)
                                          .SingleOrDefault(x => !string.IsNullOrEmpty(x));
            return AppendRoutePrefix(routeTemplate, controllerType);
        }

        protected override bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType)
        {
            if (IsUserScopedApi(controllerType) && parameterInfo.Name == "userId")
                return false;
            return true;
        }

        protected override IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !x.Parameter.GetCustomAttributes<FromBodyAttribute>().Any()).ToArray();
        }

        protected override IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && x.Parameter.GetCustomAttributes<FromBodyAttribute>().Any());
        }

        protected override IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType)
        {
            return controllerType.Type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(m => !m.IsSpecialName)
                                 .Where(x => x.DeclaringType == controllerType)
                                 .Select(x => new MethodWrapper(x))
                                 .ToArray();
        }

        private string AppendRoutePrefix(string routeTemplate, ITypeInfo controllerType)
        {
            var routeAttribute = controllerType.Type.GetCustomAttribute<RouteAttribute>();
            var fullRoute = (routeAttribute == null ? "" : routeAttribute.Template + "/") + routeTemplate;
            if (IsUserScopedApi(controllerType))
                return fullRoute.Substring("v1/user/{userId}/".Length);
            return fullRoute.Substring("v1/".Length);
        }

        private bool IsUserScopedApi(ITypeInfo controller)
        {
            var route = controller.Type.GetCustomAttribute<RouteAttribute>();
            return route?.Template.StartsWith("v1/user/{userId}") ?? false;
        }
    }
}