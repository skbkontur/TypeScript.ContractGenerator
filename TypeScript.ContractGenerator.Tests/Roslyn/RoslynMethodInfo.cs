using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Roslyn
{
    public class RoslynMethodInfo : IMethodInfo
    {
        public RoslynMethodInfo(IMethodSymbol methodSymbol)
        {
            this.methodSymbol = methodSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return methodSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public MethodInfo Method { get; }
        public string Name => methodSymbol.Name;
        public ITypeInfo ReturnType => new RoslynTypeInfo(methodSymbol.ReturnType);

        public IParameterInfo[] GetParameters()
        {
            return methodSymbol.Parameters.Select(x => (IParameterInfo)new RoslynParameterInfo(x)).ToArray();
        }

        private readonly IMethodSymbol methodSymbol;
    }
}