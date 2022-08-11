using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

using BindingFlags = System.Reflection.BindingFlags;

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
            return TypeInfo.From<ControllerBase>().IsAssignableFrom(type);
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

        protected override ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(TypeInfo.From(typeof(Task<>))) || genericTypeDefinition.Equals(TypeInfo.From(typeof(ActionResult<>))))
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(TypeInfo.From<Task>()) || typeInfo.Equals(TypeInfo.From<ActionResult>()))
                return TypeInfo.From(typeof(void));

            return typeInfo;
        }

        protected override BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo)
        {
            if (methodInfo.GetAttributes(TypeInfo.From<HttpGetAttribute>()).Any())
                return BaseApiMethod.Get;

            if (methodInfo.GetAttributes(TypeInfo.From<HttpPostAttribute>()).Any())
                return BaseApiMethod.Post;

            if (methodInfo.GetAttributes(TypeInfo.From<HttpPutAttribute>()).Any())
                return BaseApiMethod.Put;

            if (methodInfo.GetAttributes(TypeInfo.From<HttpDeleteAttribute>()).Any())
                return BaseApiMethod.Delete;

            if (methodInfo.GetAttributes(TypeInfo.From<HttpPatchAttribute>()).Any())
                return BaseApiMethod.Patch;

            throw new NotSupportedException($"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.DeclaringType?.Name}");
        }

        protected override string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo)
        {
            var routeTemplate = methodInfo.GetAttributes(false)
                                          .Select(x => x.GetValue("Template", ""))
                                          .SingleOrDefault(x => !string.IsNullOrEmpty(x));
            return AppendRoutePrefix(routeTemplate ?? string.Empty, controllerType);
        }

        protected override bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType)
        {
            if (IsUserScopedApi(controllerType) && parameterInfo.Name == "userId")
                return false;
            return true;
        }

        protected override IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !x.GetAttributes(TypeInfo.From<FromBodyAttribute>()).Any()).ToArray();
        }

        protected override IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && x.GetAttributes(TypeInfo.From<FromBodyAttribute>()).Any());
        }

        protected override IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToArray();
        }

        private static string GetApiBaseName(IAttributeProvider controllerType)
        {
            return IsUserScopedApi(controllerType) ? "UserApiBase" : "ApiBase";
        }

        private static string AppendRoutePrefix(string routeTemplate, IAttributeProvider controllerType)
        {
            var routeAttribute = controllerType.GetAttributes(TypeInfo.From<RouteAttribute>()).SingleOrDefault();
            var fullRoute = (routeAttribute == null ? "" : routeAttribute.GetValue("Template", "") + "/") + routeTemplate;
            if (IsUserScopedApi(controllerType))
                return fullRoute.Substring("v1/user/{userId}/".Length);
            return fullRoute.Substring("v1/".Length);
        }

        private static bool IsUserScopedApi(IAttributeProvider controller)
        {
            var route = controller.GetAttributes(TypeInfo.From<RouteAttribute>()).SingleOrDefault();
            var template = route?.GetValue<string?>("Template", null);
            return template?.StartsWith("v1/user/{userId}") ?? false;
        }
    }
}