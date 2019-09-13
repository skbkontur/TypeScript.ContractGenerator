using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public abstract class ApiControllerTypeBuildingContextBase : TypeBuildingContext
    {
        public ApiControllerTypeBuildingContextBase(TypeScriptUnit unit, Type type)
            : base(unit, type)
        {
        }

        protected virtual TypeLocation GetApiBase(Type controllerType)
        {
            return new TypeLocation
                {
                    Name = "ApiBase",
                    Location = "../apiBase/ApiBase", 
                };
        }

        protected virtual string GetApiName(Type controllerType)
        {
            return new Regex("(Api)?Controller").Replace(controllerType.Name, "Api");
        }

        protected virtual Type ResolveReturnType(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>))
                    return ResolveReturnType(type.GetGenericArguments()[0]);
            }

            if (type == typeof(Task))
                return typeof(void);
            return type;
        }

        protected virtual TypeScriptType ResolveReturnType(MethodInfo methodInfo, Func<ICustomAttributeProvider, Type, TypeScriptType> buildAndImportType)
        {
            return null;
        }

        protected virtual bool PassParameterToCall(ParameterInfo parameterInfo, Type controllerType) => true;
        
        protected virtual TypeScriptStatement WrapCall(MethodInfo methodInfo, TypeScriptReturnStatement call) => call;

        protected virtual string GetApiClassName(string apiName) => apiName;
        
        protected abstract BaseApiMethod ResolveBaseApiMethod(MethodInfo methodInfo);
        protected abstract string BuildRoute(Type controllerType, MethodInfo methodInfo);
        protected abstract ParameterInfo[] GetQueryParameters(ParameterInfo[] parameters, Type controllerType);
        protected abstract ParameterInfo GetBody(ParameterInfo[] parameters, Type controllerType);
        protected abstract MethodInfo[] GetMethodsToImplement(Type controllerType);

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = GenerateInternalApiController(Unit, Type, (x, y) => typeGenerator.BuildAndImportType(Unit, x, y));
            base.Initialize(typeGenerator);
        }

        private TypeScriptTypeDeclaration GenerateInternalApiController(TypeScriptUnit targetUnit, Type type, Func<ICustomAttributeProvider, Type, TypeScriptType> buildAndImportType)
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

        private IEnumerable<TypeScriptClassMemberDefinition> BuildApiImplMember(MethodInfo methodInfo, Func<ICustomAttributeProvider, Type, TypeScriptType> buildAndImportType, Type controllerType)
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

        private TypeScriptType GetMethodResult(MethodInfo methodInfo, Func<ICustomAttributeProvider, Type, TypeScriptType> buildAndImportType)
        {
            if (methodResults.TryGetValue(methodInfo, out var result))
                return result;
            return methodResults[methodInfo] = ResolveReturnType(methodInfo, buildAndImportType) ?? new TypeScriptPromiseOfType(buildAndImportType(methodInfo, ResolveReturnType(methodInfo.ReturnType)));
        }

        private Dictionary<MethodInfo, TypeScriptType> methodResults = new Dictionary<MethodInfo, TypeScriptType>();

        private TypeScriptReturnStatement CreateCall(MethodInfo methodInfo, Type controllerType)
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

        private TypeScriptReturnStatement GenerateMethodCallWithBody(MethodInfo methodInfo, string methodName, Type controllerType)
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

        private TypeScriptExpression GetBodyExpression(MethodInfo methodInfo, string methodName, Type controllerType)
        {
            var bodyParameter = GetBody(methodInfo.GetParameters(), controllerType);

            return GenerateCustomBody(methodInfo, methodName, controllerType) ?? GenerateConstructBody(bodyParameter);
        }

        private static TypeScriptExpression GenerateConstructBody(ParameterInfo parameter)
        {
            if (parameter == null)
                return new TypeScriptObjectLiteral();

            if (parameter.ParameterType.IsArray)
                return new TypeScriptVariableReference(parameter.Name);

            return new TypeScriptObjectLiteral(
                new TypeScriptObjectLiteralSpread(new TypeScriptVariableReference(parameter.Name))
                );
        }

        protected virtual TypeScriptExpression GenerateCustomBody(MethodInfo methodInfo, string methodName, Type controllerType)
        {
            return null;
        }

        private static TypeScriptExpression GenerateConstructGetParams(ParameterInfo[] parameters, string routeTemplate)
        {
            var literalProperties = parameters
                .Select<ParameterInfo, TypeScriptObjectLiteralInitializer>(
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

        private IEnumerable<TypeScriptInterfaceFunctionMember> BuildApiInterfaceMember(MethodInfo methodInfo, Func<ICustomAttributeProvider, Type, TypeScriptType> buildAndImportType, Type controllerType)
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
    }
}