using System;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public interface IApiCustomization
    {
        public ApiBaseLocation GetApiBase(ITypeInfo type);

        string GetApiClassName(ITypeInfo type);
        string GetApiInterfaceName(ITypeInfo type);
        string GetMethodName(IMethodInfo methodInfo);

        IMethodInfo[] GetApiMethods(ITypeInfo type);
        IParameterInfo[] GetMethodParameters(IMethodInfo methodInfo);

        bool IsUrlMethod(IMethodInfo methodInfo);
        bool IsAsyncMethod(IMethodInfo methodInfo);
        string GetMethodVerb(IMethodInfo methodInfo);
        string GetMethodRoute(IMethodInfo methodInfo);
        TypeScriptType GetMethodResultType(IMethodInfo methodInfo, Func<ITypeInfo, TypeScriptType> buildAndImportType);
        TypeScriptExpression? GetMethodBodyExpression(IMethodInfo methodInfo);
    }
}