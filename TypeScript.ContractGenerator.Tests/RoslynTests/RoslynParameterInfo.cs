using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynParameterInfo : IParameterInfo
    {
        public RoslynParameterInfo(IParameterSymbol parameterSymbol)
        {
            this.parameterSymbol = parameterSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return parameterSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public ParameterInfo Parameter { get; }
        public string Name => parameterSymbol.Name;
        public ITypeInfo ParameterType => new RoslynTypeInfo(parameterSymbol.Type);

        private readonly IParameterSymbol parameterSymbol;
    }
}