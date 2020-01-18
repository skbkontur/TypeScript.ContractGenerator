using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynPropertyInfo : IPropertyInfo
    {
        public RoslynPropertyInfo(IPropertySymbol propertySymbol)
        {
            PropertySymbol = propertySymbol;
        }

        public bool IsNameDefined(string name)
        {
            return PropertySymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public IPropertySymbol PropertySymbol { get; }

        public string Name => PropertySymbol.Name;
        public ITypeInfo PropertyType => RoslynTypeInfo.From(PropertySymbol.Type);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(PropertySymbol.ContainingType);
    }
}