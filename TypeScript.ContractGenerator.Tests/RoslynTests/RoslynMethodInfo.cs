using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynMethodInfo : IMethodInfo
    {
        public RoslynMethodInfo(IMethodSymbol methodSymbol)
        {
            MethodSymbol = methodSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return MethodSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public IMethodSymbol MethodSymbol { get; }

        public string Name => MethodSymbol.Name;
        public ITypeInfo ReturnType => RoslynTypeInfo.From(MethodSymbol.ReturnType);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(MethodSymbol.ContainingType);

        public IParameterInfo[] GetParameters()
        {
            return MethodSymbol.Parameters.Select(x => (IParameterInfo)new RoslynParameterInfo(x)).ToArray();
        }
    }
}