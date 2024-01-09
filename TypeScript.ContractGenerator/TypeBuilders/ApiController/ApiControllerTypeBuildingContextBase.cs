using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

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

        protected virtual TypeScriptType? ResolveReturnType(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType)
        {
            return null;
        }

        protected virtual string GetMethodName(IMethodInfo methodInfo)
        {
            return methodInfo.Name.ToLowerCamelCase();
        }

        protected virtual bool IsAsyncMethod(IMethodInfo methodInfo)
        {
            return true;
        }

        protected virtual bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType) => true;

        protected virtual TypeScriptStatement WrapCall(IMethodInfo methodInfo, TypeScriptReturnStatement call) => call;

        protected virtual string GetApiClassName(string apiName) => apiName;

        protected abstract BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo);
        protected abstract string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo);
        protected abstract IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType);
        protected abstract IParameterInfo? GetBody(IParameterInfo[] parameters, ITypeInfo controllerType);
        protected abstract IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType);

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = GenerateInternalApiController(Unit, Type, x => typeGenerator.BuildAndImportType(Unit, x));
            base.Initialize(typeGenerator);
        }

        private TypeScriptTypeDeclaration GenerateInternalApiController(TypeScriptUnit targetUnit, ITypeInfo type, Func<ITypeInfo, TypeScriptType> buildAndImportType)
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

        protected virtual IEnumerable<TypeScriptClassMemberDefinition> BuildApiImplMember(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType, ITypeInfo controllerType)
        {
            var functionDefinition = new TypeScriptFunctionDefinition
                {
                    IsAsync = IsAsyncMethod(methodInfo),
                    Result = GetMethodResult(methodInfo, buildAndImportType),
                    Body = {WrapCall(methodInfo, CreateCall(methodInfo, controllerType))}
                };
            functionDefinition.Arguments.AddRange(
                methodInfo.GetParameters().Where(x => PassParameterToCall(x, controllerType)).Select(x => new TypeScriptArgumentDeclaration
                    {
                        Name = x.Name,
                        Type = buildAndImportType(x.ParameterType)
                    })
            );
            yield return
                new TypeScriptClassMemberDefinition
                    {
                        Name = GetMethodName(methodInfo),
                        Definition = functionDefinition
                    };
        }

        private TypeScriptType GetMethodResult(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType)
        {
            return ResolveReturnType(methodInfo, buildAndImportType) ?? new TypeScriptPromiseOfType(buildAndImportType(ResolveReturnType(methodInfo.ReturnType)));
        }

        protected virtual TypeScriptReturnStatement CreateCall(IMethodInfo methodInfo, ITypeInfo controllerType)
        {
            var verb = ResolveBaseApiMethod(methodInfo);
            return GenerateMethodCallWithBody(methodInfo, $"make{verb}Request", controllerType);
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

        private static TypeScriptExpression GenerateConstructBody(IParameterInfo? parameter)
        {
            if (parameter == null)
                return new TypeScriptObjectLiteral();

            if (parameter.ParameterType.IsArray)
                return new TypeScriptVariableReference(parameter.Name);

            return new TypeScriptObjectLiteral(
                new TypeScriptObjectLiteralSpread(new TypeScriptVariableReference(parameter.Name))
            );
        }

        protected virtual TypeScriptExpression? GenerateCustomBody(IMethodInfo methodInfo, string methodName, ITypeInfo controllerType)
        {
            return null;
        }

        private static TypeScriptExpression GenerateConstructGetParams(IParameterInfo[] parameters, string routeTemplate)
        {
            var literalProperties = parameters
                                    .Where(x => !routeTemplate.Contains("{" + x.Name + "}"))
                                    .Select(parameter => (TypeScriptObjectLiteralInitializer)new TypeScriptObjectLiteralProperty(
                                                new TypeScriptStringLiteral(parameter.Name),
                                                new TypeScriptVariableReference(parameter.Name)))
                                    .ToArray();
            var result = new TypeScriptObjectLiteral(literalProperties);
            return result;
        }

        protected virtual IEnumerable<TypeScriptInterfaceFunctionMember> BuildApiInterfaceMember(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType, ITypeInfo controllerType)
        {
            var result = new TypeScriptInterfaceFunctionMember(
                GetMethodName(methodInfo),
                GetMethodResult(methodInfo, buildAndImportType)
            );
            result.Arguments.AddRange(
                methodInfo.GetParameters()
                          .Where(x => PassParameterToCall(x, controllerType))
                          .Select(x => new TypeScriptArgumentDeclaration
                              {
                                  Name = x.Name,
                                  Type = buildAndImportType(x.ParameterType)
                              })
            );
            yield return result;
        }
    }
}