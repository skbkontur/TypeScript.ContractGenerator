using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynPropertyInfo : IPropertyInfo
    {
        public RoslynPropertyInfo(IPropertySymbol propertySymbol)
        {
            PropertySymbol = propertySymbol;
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return PropertySymbol.GetAttributesInfo();
        }

        public IPropertySymbol PropertySymbol { get; }

        public string Name => PropertySymbol.Name;
        public ITypeInfo PropertyType => RoslynTypeInfo.From(PropertySymbol.Type).WithMemberInfo(this);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(PropertySymbol.ContainingType);
    }
}