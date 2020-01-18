using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynFieldInfo : IFieldInfo
    {
        public RoslynFieldInfo(IFieldSymbol fieldSymbol)
        {
            FieldSymbol = fieldSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return FieldSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public IFieldSymbol FieldSymbol { get; }

        public string Name => FieldSymbol.Name;
        public ITypeInfo FieldType => RoslynTypeInfo.From(FieldSymbol.Type);
        public ITypeInfo DeclaringType => RoslynTypeInfo.From(FieldSymbol.ContainingType);
    }
}