using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynFieldInfo : IFieldInfo
    {
        public RoslynFieldInfo(IFieldSymbol fieldSymbol)
        {
            FieldSymbol = fieldSymbol;
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return FieldSymbol.GetAttributesInfo();
        }

        public IFieldSymbol FieldSymbol { get; }

        public string Name => FieldSymbol.Name;
        public ITypeInfo FieldType => RoslynTypeInfo.From(FieldSymbol.Type).WithMemberInfo(this);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(FieldSymbol.ContainingType);
    }
}