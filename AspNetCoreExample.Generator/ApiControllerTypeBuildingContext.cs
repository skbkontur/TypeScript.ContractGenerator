using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace AspNetCoreExample.Generator
{
    public class ApiControllerTypeBuildingContext : ApiControllerTypeBuildingContextBase
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return typeof(ControllerBase).IsAssignableFrom(type.Type);
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
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(new TypeWrapper(typeof(Task<>))) || genericTypeDefinition.Equals(new TypeWrapper(typeof(ActionResult<>))))
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(TypeInfo.FromType<Task>()))
                return new TypeWrapper(typeof(void));
            if (typeInfo.Equals(TypeInfo.FromType<ActionResult>()))
                return new TypeWrapper(typeof(void));
            return typeInfo;
        }

        protected override BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttributes<HttpGetAttribute>().Any())
                return BaseApiMethod.Get;

            if (methodInfo.GetCustomAttributes<HttpPostAttribute>().Any())
                return BaseApiMethod.Post;

            if (methodInfo.GetCustomAttributes<HttpPutAttribute>().Any())
                return BaseApiMethod.Put;

            if (methodInfo.GetCustomAttributes<HttpDeleteAttribute>().Any())
                return BaseApiMethod.Delete;

            throw new NotSupportedException($"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.Method.DeclaringType?.Name}");
        }

        protected override string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo)
        {
            var routeTemplate = methodInfo.GetCustomAttributes(false)
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
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !x.GetCustomAttributes<FromBodyAttribute>().Any()).ToArray();
        }

        protected override IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && x.GetCustomAttributes<FromBodyAttribute>().Any());
        }

        protected override IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType)
        {
            return controllerType.Type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(m => !m.IsSpecialName)
                                 .Where(x => x.DeclaringType == controllerType.Type)
                                 .Select(x => (IMethodInfo)new MethodWrapper(x))
                                 .ToArray();
        }

        private string AppendRoutePrefix(string routeTemplate, ITypeInfo controllerType)
        {
            var routeAttribute = controllerType.GetCustomAttributes<RouteAttribute>().SingleOrDefault();
            var fullRoute = (routeAttribute == null ? "" : routeAttribute.Template + "/") + routeTemplate;
            if (IsUserScopedApi(controllerType))
                return fullRoute.Substring("v1/user/{userId}/".Length);
            return fullRoute.Substring("v1/".Length);
        }

        private bool IsUserScopedApi(ITypeInfo controller)
        {
            var route = controller.GetCustomAttributes<RouteAttribute>().SingleOrDefault();
            return route?.Template.StartsWith("v1/user/{userId}") ?? false;
        }
    }
}