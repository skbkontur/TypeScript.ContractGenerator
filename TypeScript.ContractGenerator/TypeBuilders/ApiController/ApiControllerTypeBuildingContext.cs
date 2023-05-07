using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public class ApiControllerTypeBuildingContext : ApiControllerTypeBuildingContextBase
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        protected override TypeLocation GetApiBase(ITypeInfo controllerType)
        {
            return new TypeLocation
                {
                    Name = "ApiBase",
                    Location = "ApiBase/ApiBase",
                };
        }

        protected override ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(TypeInfo.From(typeof(Task<>))) ||
                    genericTypeDefinition.Name == KnownTypeNames.ActionResultOfT ||
                    genericTypeDefinition.Name == KnownTypeNames.ActionResult)
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(TypeInfo.From<Task>()) || typeInfo.Name == KnownTypeNames.ActionResult)
                return TypeInfo.From(typeof(void));

            return typeInfo;
        }

        protected override BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo)
        {
            var attributes = methodInfo.GetAttributes(inherit : false);

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpGet)))
                return BaseApiMethod.Get;

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPost)))
                return BaseApiMethod.Post;

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPut)))
                return BaseApiMethod.Put;

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpDelete)))
                return BaseApiMethod.Delete;

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPatch)))
                return BaseApiMethod.Patch;

            throw new NotSupportedException(
                $"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.DeclaringType?.Name}");
        }

        protected override string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo)
        {
            var fullRoute = RouteTemplateHelper.FindFullRouteTemplate(controllerType, methodInfo)?.ValueWithoutConstraints ?? "";
            if (fullRoute.StartsWith("/"))
                return fullRoute;

            return "/" + fullRoute;
        }

        protected override IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !IsFromBody(x))
                             .ToArray();
        }

        protected override IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && IsFromBody(x));
        }

        protected override IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                 .Where(m => m.GetAttributes(inherit : false)
                                              .Any(a => KnownTypeNames.HttpAttributeNames.Any(a.HasName)))
                                 .ToArray();
        }

        protected override bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType)
        {
            return !parameterInfo.ParameterType.Equals(TypeInfo.From<CancellationToken>());
        }

        protected override string GetMethodName(IMethodInfo methodInfo)
        {
            return IsUrlMethod(methodInfo) ? $"get{methodInfo.Name}Url" : methodInfo.Name.ToLowerCamelCase();
        }

        protected override TypeScriptReturnStatement CreateCall(IMethodInfo methodInfo, ITypeInfo controllerType)
        {
            if (!IsUrlMethod(methodInfo))
                return base.CreateCall(methodInfo, controllerType);

            var routeTemplate = BuildRoute(controllerType, methodInfo);
            return new TypeScriptReturnStatement(new TypeScriptTemplateStringLiteral(routeTemplate.Replace("{", "${")));
        }

        protected override bool IsAsyncMethod(IMethodInfo methodInfo)
        {
            return !IsUrlMethod(methodInfo);
        }

        protected override TypeScriptType? ResolveReturnType(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType)
        {
            return IsUrlMethod(methodInfo) ? new TypeScriptTypeReference("string") : null;
        }

        private static bool IsFromBody(IParameterInfo parameterInfo)
        {
            return parameterInfo.GetAttributes(inherit : false).Any(x => x.HasName(KnownTypeNames.Attributes.FromBody));
        }

        private static bool IsUrlMethod(IMethodInfo methodInfo)
        {
            return methodInfo.GetAttributes(TypeInfo.From<UrlOnlyAttribute>()).Any();
        }
    }
}