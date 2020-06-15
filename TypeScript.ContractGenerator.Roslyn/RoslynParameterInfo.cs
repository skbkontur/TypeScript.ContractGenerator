using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynParameterInfo : IParameterInfo
    {
        public RoslynParameterInfo(IParameterSymbol parameterSymbol)
        {
            ParameterSymbol = parameterSymbol;
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return ParameterSymbol.GetAttributesInfo();
        }

        public IParameterSymbol ParameterSymbol { get; }

        public string Name => ParameterSymbol.Name;
        public ITypeInfo ParameterType => RoslynTypeInfo.From(ParameterSymbol.Type).WithMemberInfo(this);
        public IMethodInfo Method => new RoslynMethodInfo((IMethodSymbol)ParameterSymbol.ContainingSymbol);
    }
}