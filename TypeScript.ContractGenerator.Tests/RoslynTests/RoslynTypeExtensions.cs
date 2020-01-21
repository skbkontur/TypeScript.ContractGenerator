using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public static class RoslynTypeExtensions
    {
        public static IAttributeInfo[] GetAttributesInfo(this ISymbol symbol)
        {
            return symbol.GetAttributes().Select(x => (IAttributeInfo)new RoslynAttributeInfo(x)).ToArray();
        }
    }
}