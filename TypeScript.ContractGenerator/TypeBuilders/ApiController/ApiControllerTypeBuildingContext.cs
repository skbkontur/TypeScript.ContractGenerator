using System;
using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public class ApiControllerTypeBuildingContext : TypeBuildingContext
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type, IApiCustomization? apiCustomization = null)
            : base(unit, type)
        {
            ApiCustomization = apiCustomization ?? new DefaultApiCustomization();
        }

        protected IApiCustomization ApiCustomization { get; }
        protected Func<ITypeInfo, TypeScriptType> BuildAndImportType { get; private set; }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            BuildAndImportType = t => typeGenerator.BuildAndImportType(Unit, t);

            var baseApi = ApiCustomization.GetApiBase(Type);
            var urlTag = ApiCustomization.GetUrlTag(Type);

            Unit.AddSymbolImport(baseApi.Name, baseApi.Location);
            Unit.AddSymbolImport(urlTag.Name, urlTag.Location);

            var apiName = ApiCustomization.GetApiClassName(Type);
            var interfaceName = ApiCustomization.GetApiInterfaceName(Type);
            var apiMethods = ApiCustomization.GetApiMethods(Type);

            var apiInterfaceDefinition = new TypeScriptInterfaceDefinition();
            apiInterfaceDefinition.Members.AddRange(apiMethods.SelectMany(BuildApiInterfaceMember));

            var apiClassDefinition = new TypeScriptClassDefinition
                {
                    BaseClass = new TypeScriptTypeReference(baseApi.Name),
                    ImplementedInterfaces = new TypeScriptType[] {new TypeScriptTypeReference(interfaceName)},
                };

            apiClassDefinition.Members.AddRange(apiMethods.SelectMany(BuildApiImplMember));

            Unit.Body.Add(new TypeScriptExportStatement
                {
                    Declaration = new TypeScriptClassDeclaration
                        {
                            Name = apiName,
                            Defintion = apiClassDefinition
                        }
                });

            foreach (var statement in BuildAdditionalStatements())
            {
                Unit.Body.Add(statement);
            }

            Declaration = new TypeScriptInterfaceDeclaration
                {
                    Name = interfaceName,
                    Definition = apiInterfaceDefinition
                };

            base.Initialize(typeGenerator);
        }

        protected virtual IEnumerable<TypeScriptClassMemberDefinition> BuildApiImplMember(IMethodInfo methodInfo)
        {
            var functionDefinition = new TypeScriptFunctionDefinition
                {
                    IsAsync = ApiCustomization.IsAsyncMethod(methodInfo),
                    Result = ApiCustomization.GetMethodResultType(methodInfo, BuildAndImportType),
                    Body = {CreateCall(methodInfo)}
                };

            functionDefinition.Arguments.AddRange(
                ApiCustomization
                    .GetMethodParameters(methodInfo)
                    .Select(x => new TypeScriptArgumentDeclaration
                        {
                            Name = x.Name,
                            Type = BuildAndImportType(x.ParameterType)
                        })
            );

            yield return new TypeScriptClassMemberDefinition
                {
                    Name = ApiCustomization.GetMethodName(methodInfo),
                    Definition = functionDefinition
                };
        }

        protected virtual TypeScriptReturnStatement CreateCall(IMethodInfo methodInfo)
        {
            var route = ApiCustomization.GetMethodRoute(methodInfo);
            var routeExpression = new TypeScriptTemplateStringLiteral(route.Replace("{", "${"), new TypeScriptVariableReference("url"));

            if (ApiCustomization.IsUrlMethod(methodInfo))
            {
                return new TypeScriptReturnStatement(routeExpression);
            }

            var bodyExpression = ApiCustomization.GetMethodBodyExpression(methodInfo);
            var arguments = bodyExpression == null
                                ? new[] {routeExpression}
                                : new[] {routeExpression, bodyExpression};

            return new TypeScriptReturnStatement(
                new TypeScriptMethodCallExpression(new TypeScriptThisReference(), ApiCustomization.GetMethodVerb(methodInfo), arguments)
            );
        }

        protected virtual IEnumerable<TypeScriptInterfaceFunctionMember> BuildApiInterfaceMember(IMethodInfo methodInfo)
        {
            var result = new TypeScriptInterfaceFunctionMember(
                ApiCustomization.GetMethodName(methodInfo),
                ApiCustomization.GetMethodResultType(methodInfo, BuildAndImportType)
            );

            result.Arguments.AddRange(
                ApiCustomization
                    .GetMethodParameters(methodInfo)
                    .Select(x => new TypeScriptArgumentDeclaration
                        {
                            Name = x.Name,
                            Type = BuildAndImportType(x.ParameterType)
                        })
            );

            yield return result;
        }

        protected virtual IEnumerable<TypeScriptStatement> BuildAdditionalStatements()
        {
            yield break;
        }
    }
}