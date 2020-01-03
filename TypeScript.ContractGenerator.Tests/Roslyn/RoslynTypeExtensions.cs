using System.Linq;

using Microsoft.CodeAnalysis;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Roslyn
{
    public static class RoslynTypeExtensions
    {
        public static bool IsNameDefined(this ISymbol symbol, string name)
        {
            return symbol.GetAttributes().Any(x => x.AttributeClass.Name == name || x.AttributeClass.Name == name.Replace("Attribute", ""));
        }
    }
}