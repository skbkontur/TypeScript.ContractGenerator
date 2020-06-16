using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynAssemblyInfo : IAssemblyInfo
    {
        public RoslynAssemblyInfo(IAssemblySymbol assemblySymbol)
        {
            AssemblySymbol = assemblySymbol;
        }

        public IAssemblySymbol AssemblySymbol { get; }

        public ITypeInfo[] GetTypes()
        {
            return AssemblySymbol.GlobalNamespace.GetAllTypes().Select(RoslynTypeInfo.From).ToArray();
        }
    }
}