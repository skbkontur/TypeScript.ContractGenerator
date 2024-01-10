using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public class DefaultApiCustomization : IApiCustomization
    {
        public TypeLocation GetApiBase(ITypeInfo type) => new TypeLocation
            {
                Name = "ApiBase",
                Location = "ApiBase/ApiBase",
            };

        public TypeLocation GetUrlTag(ITypeInfo type) => new TypeLocation
            {
                Name = "url",
                Location = "ApiBase/ApiBase",
            };

        public string GetApiClassName(ITypeInfo type) => new Regex("(Api)?Controller").Replace(type.Name, "Api");
        public string GetApiInterfaceName(ITypeInfo type) => "I" + GetApiClassName(type);

        public string GetMethodName(IMethodInfo methodInfo) => IsUrlMethod(methodInfo)
                                                                   ? $"urlFor{methodInfo.Name}"
                                                                   : methodInfo.Name.ToLowerCamelCase();

        public IMethodInfo[] GetApiMethods(ITypeInfo type) =>
            type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m.GetAttributes(inherit : false).Any(a => KnownTypeNames.HttpAttributeNames.Any(a.HasName)))
                .ToArray();

        public IParameterInfo[] GetMethodParameters(IMethodInfo methodInfo) =>
            methodInfo.GetParameters()
                      .Where(x => !x.ParameterType.Equals(TypeInfo.From<CancellationToken>()))
                      .ToArray();

        public bool IsUrlMethod(IMethodInfo methodInfo)
        {
            return GetMethodVerb(methodInfo) == "makeGetRequest" && ResolveReturnType(methodInfo.ReturnType).Equals(TypeInfo.From(typeof(void)));
        }

        public bool IsAsyncMethod(IMethodInfo methodInfo) => false;

        public string GetMethodVerb(IMethodInfo methodInfo)
        {
            var attributes = methodInfo.GetAttributes(inherit : false);

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpGet)))
                return "makeGetRequest";

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPost)))
                return "makePostRequest";

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPut)))
                return "makePutRequest";

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpDelete)))
                return "makeDeleteRequest";

            if (attributes.Any(x => x.HasName(KnownTypeNames.Attributes.HttpPatch)))
                return "makePatchRequest";

            throw new NotSupportedException($"Unresolved http verb for method {methodInfo.Name} at controller {methodInfo.DeclaringType?.Name}");
        }

        public string GetMethodRoute(IMethodInfo methodInfo)
        {
            var path = RouteTemplateHelper.GetRouteTemplate(methodInfo.DeclaringType!, methodInfo);

            var routeParameters = new HashSet<string>(
                Regex.Matches(path, @"{(\w+)}")
                     .Cast<Match>()
                     .Where(x => x.Success && x.Groups.Count > 1 && x.Groups[1].Success)
                     .Select(x => x.Groups[1].Value)
            );

            var queryParams = GetMethodParameters(methodInfo)
                              .Where(x => !IsFromBody(x) && !routeParameters.Contains(x.Name))
                              .Select(x => $"{x.Name}={{{x.Name}}}")
                              .ToArray();

            var query = queryParams.Any()
                            ? "?" + string.Join("&", queryParams)
                            : string.Empty;

            return $"{path}{query}";
        }

        public TypeScriptType GetMethodResultType(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType)
        {
            return IsUrlMethod(methodInfo)
                       ? (TypeScriptType)new TypeScriptTypeReference("string")
                       : new TypeScriptPromiseOfType(buildAndImportType(ResolveReturnType(methodInfo.ReturnType)));
        }

        public TypeScriptExpression? GetMethodBodyExpression(IMethodInfo methodInfo)
        {
            var parameter = GetMethodParameters(methodInfo).SingleOrDefault(IsFromBody);
            return parameter == null
                       ? null
                       : new TypeScriptVariableReference(parameter.Name);
        }

        private static bool IsFromBody(IParameterInfo parameterInfo)
        {
            return parameterInfo
                   .GetAttributes(inherit : false)
                   .Any(a => a.HasName(KnownTypeNames.Attributes.FromBody));
        }

        private static ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(TypeInfo.From(typeof(Task<>))) ||
                    genericTypeDefinition.Name == KnownTypeNames.ActionResultOfT ||
                    genericTypeDefinition.Name == KnownTypeNames.ActionResult)
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(TypeInfo.From<Task>()) ||
                typeInfo.Name == KnownTypeNames.ActionResult ||
                typeInfo.Name == KnownTypeNames.IActionResult)
                return TypeInfo.From(typeof(void));

            return typeInfo;
        }
    }
}