using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynMethodInfo : IMethodInfo
    {
        public RoslynMethodInfo(IMethodSymbol methodSymbol)
        {
            MethodSymbol = methodSymbol;
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return MethodSymbol.GetAttributesInfo();
        }

        public IMethodSymbol MethodSymbol { get; }

        public string Name => MethodSymbol.Name;
        public ITypeInfo ReturnType => RoslynTypeInfo.From(MethodSymbol.ReturnType).WithMemberInfo(this);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(MethodSymbol.ContainingType);

        public IParameterInfo[] GetParameters()
        {
            return MethodSymbol.Parameters.Select(x => (IParameterInfo)new RoslynParameterInfo(x)).ToArray();
        }
    }
}