using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynParameterInfo : IParameterInfo
    {
        public RoslynParameterInfo(IParameterSymbol parameterSymbol)
        {
            ParameterSymbol = parameterSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return ParameterSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public IParameterSymbol ParameterSymbol { get; }

        public string Name => ParameterSymbol.Name;
        public ITypeInfo ParameterType => RoslynTypeInfo.From(ParameterSymbol.Type);
    }
}