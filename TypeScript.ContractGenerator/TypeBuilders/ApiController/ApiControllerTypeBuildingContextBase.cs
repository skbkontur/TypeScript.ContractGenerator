using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public abstract class ApiControllerTypeBuildingContextBase : TypeBuildingContext
    {
        public ApiControllerTypeBuildingContextBase(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        protected virtual TypeLocation GetApiBase(ITypeInfo controllerType)
        {
            return new TypeLocation
                {
                    Name = "ApiBase",
                    Location = "../apiBase/ApiBase",
                };
        }

        protected virtual string GetApiName(ITypeInfo controllerType)
        {
            return new Regex("(Api)?Controller").Replace(controllerType.Name, "Api");
        }

        protected virtual ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(TypeInfo.From(typeof(Task<>))))
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(TypeInfo.From<Task>()))
                return TypeInfo.From(typeof(void));
            return typeInfo;
        }

        protected virtual TypeScriptType ResolveReturnType(IMethodInfo methodInfo, Func<IAttributeProvider, ITypeInfo, TypeScriptType> buildAndImportType)
        {
            return null;
        }

        protected virtual bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType) => true;

        protected virtual TypeScriptStatement WrapCall(IMethodInfo methodInfo, TypeScriptReturnStatement call) => call;

        protected virtual string GetApiClassName(string apiName) => apiName;

        protected abstract BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo);
        protected abstract string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo);
        protected abstract IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType);
        protected abstract IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType);
        protected abstract IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType);

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = GenerateInternalApiController(Unit, Type, (x, y) => typeGenerator.BuildAndImportType(Unit, x, y));
            base.Initialize(typeGenerator);
        }

        private TypeScriptTypeDeclaration GenerateInternalApiController(TypeScriptUnit targetUnit, ITypeInfo type, Func<IAttributeProvider, ITypeInfo, TypeScriptType> buildAndImportType)
        {
            var baseApi = GetApiBase(type);
            var apiName = GetApiName(type);
            var interfaceName = "I" + apiName;
            var methodInfos = GetMethodsToImplement(type);

            var definition = new TypeScriptInterfaceDefinition();
            definition.Members.AddRange(methodInfos
                                            .SelectMany(x => BuildApiInterfaceMember(x, buildAndImportType, type)));
            targetUnit.AddSymbolImport(baseApi.Name, baseApi.Location);

            var interfaceDeclaration = new TypeScriptInterfaceDeclaration
                {
                    Name = interfaceName,
                    Definition = definition
                };
            var typeScriptClassDefinition = new TypeScriptClassDefinition
                {
                    BaseClass = new TypeScriptTypeReference(baseApi.Name),
                    ImplementedInterfaces = new TypeScriptType[] {new TypeScriptTypeReference(interfaceName)},
                };

            typeScriptClassDefinition.Members.AddRange(
                methodInfos
                    .SelectMany(x => BuildApiImplMember(x, buildAndImportType, type)));

            targetUnit.Body.Add(new TypeScriptExportStatement
                {
                    Declaration = new TypeScriptClassDeclaration
                        {
                            Name = GetApiClassName(apiName),
                            Defintion = typeScriptClassDefinition
                        }
                });

            return interfaceDeclaration;
        }

        private IEnumerable<TypeScriptClassMemberDefinition> BuildApiImplMember(IMethodInfo methodInfo, Func<IAttributeProvider, ITypeInfo, TypeScriptType> buildAndImportType, ITypeInfo controllerType)
        {
            var functionDefinition = new TypeScriptFunctionDefinition
                {
                    IsAsync = true,
                    Result = GetMethodResult(methodInfo, buildAndImportType),
                    Body = {WrapCall(methodInfo, CreateCall(methodInfo, controllerType))}
                };
            functionDefinition.Arguments.AddRange(
                methodInfo.GetParameters().Where(x => PassParameterToCall(x, controllerType)).Select(x => new TypeScriptArgumentDeclaration
                    {
                        Name = x.Name,
                        Type = buildAndImportType(x, x.ParameterType)
                    })
                );
            yield return
                new TypeScriptClassMemberDefinition
                    {
                        Name = methodInfo.Name.ToLowerCamelCase(),
                        Definition = functionDefinition
                    };
        }

        private TypeScriptType GetMethodResult(IMethodInfo methodInfo, Func<IAttributeProvider, ITypeInfo, TypeScriptType> buildAndImportType)
        {
            if (methodResults.TryGetValue(methodInfo.Method, out var result))
                return result;
            return methodResults[methodInfo.Method] = ResolveReturnType(methodInfo, buildAndImportType) ?? new TypeScriptPromiseOfType(buildAndImportType(methodInfo, ResolveReturnType(methodInfo.ReturnType)));
        }

        private TypeScriptReturnStatement CreateCall(IMethodInfo methodInfo, ITypeInfo controllerType)
        {
            var verb = ResolveBaseApiMethod(methodInfo);
            switch (verb)
            {
            case BaseApiMethod.Get:
                return GenerateMethodCallWithBody(methodInfo, "get", controllerType);
            case BaseApiMethod.Post:
                return GenerateMethodCallWithBody(methodInfo, "post", controllerType);
            case BaseApiMethod.Put:
                return GenerateMethodCallWithBody(methodInfo, "put", controllerType);
            case BaseApiMethod.Delete:
                return GenerateMethodCallWithBody(methodInfo, "delete", controllerType);
            case BaseApiMethod.Download:
                return GenerateMethodCallWithBody(methodInfo, "download", controllerType);
            case BaseApiMethod.Upload:
                return GenerateMethodCallWithBody(methodInfo, "upload", controllerType);
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private TypeScriptReturnStatement GenerateMethodCallWithBody(IMethodInfo methodInfo, string methodName, ITypeInfo controllerType)
        {
            var route = BuildRoute(controllerType, methodInfo);
            return new TypeScriptReturnStatement(
                new TypeScriptMethodCallExpression(
                    new TypeScriptThisReference(),
                    methodName,
                    new TypeScriptTemplateStringLiteral(route.Replace("{", "${")),
                    GenerateConstructGetParams(
                        GetQueryParameters(methodInfo.GetParameters(), controllerType),
                        route
                        ),
                    GetBodyExpression(methodInfo, methodName, controllerType)
                    ));
        }

        private TypeScriptExpression GetBodyExpression(IMethodInfo methodInfo, string methodName, ITypeInfo controllerType)
        {
            return GenerateCustomBody(methodInfo, methodName, controllerType) ??
                   GenerateConstructBody(GetBody(methodInfo.GetParameters(), controllerType));
        }

        private static TypeScriptExpression GenerateConstructBody(IParameterInfo parameter)
        {
            if (parameter == null)
                return new TypeScriptObjectLiteral();

            if (parameter.ParameterType.IsArray)
                return new TypeScriptVariableReference(parameter.Name);

            return new TypeScriptObjectLiteral(
                new TypeScriptObjectLiteralSpread(new TypeScriptVariableReference(parameter.Name))
                );
        }

        protected virtual TypeScriptExpression GenerateCustomBody(IMethodInfo methodInfo, string methodName, ITypeInfo controllerType)
        {
            return null;
        }

        private static TypeScriptExpression GenerateConstructGetParams(IParameterInfo[] parameters, string routeTemplate)
        {
            var literalProperties = parameters
                .Select<IParameterInfo, TypeScriptObjectLiteralInitializer>(
                    parameter =>
                        {
                            if (routeTemplate.Contains("{" + parameter.Name + "}"))
                            {
                                return null;
                            }

                            return new TypeScriptObjectLiteralProperty(
                                new TypeScriptStringLiteral(parameter.Name),
                                new TypeScriptVariableReference(parameter.Name)
                                );
                        })
                .Where(x => x != null)
                .ToArray();
            var result = new TypeScriptObjectLiteral(literalProperties);
            return result;
        }

        private IEnumerable<TypeScriptInterfaceFunctionMember> BuildApiInterfaceMember(IMethodInfo methodInfo, Func<IAttributeProvider, ITypeInfo, TypeScriptType> buildAndImportType, ITypeInfo controllerType)
        {
            var result = new TypeScriptInterfaceFunctionMember(
                methodInfo.Name.ToLowerCamelCase(),
                GetMethodResult(methodInfo, buildAndImportType)
                );
            result.Arguments.AddRange(
                methodInfo.GetParameters()
                          .Where(x => PassParameterToCall(x, controllerType))
                          .Select(x => new TypeScriptArgumentDeclaration
                              {
                                  Name = x.Name,
                                  Type = buildAndImportType(x, x.ParameterType)
                              })
                );
            yield return result;
        }

        private readonly Dictionary<MethodInfo, TypeScriptType> methodResults = new Dictionary<MethodInfo, TypeScriptType>();
    }
}