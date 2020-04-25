using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class FieldWrapper : IFieldInfo
    {
        public FieldWrapper(FieldInfo field)
        {
            Field = field;
        }

        public FieldInfo Field { get; }

        public string Name => Field.Name;
        public ITypeInfo FieldType => TypeInfo.From(Field.FieldType).WithNullabilityInfo(NullabilityInfo.From(DeclaringType, this));
        public ITypeInfo? DeclaringType => TypeInfo.From(Field.DeclaringType);

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return Field.GetAttributes(inherit);
        }
    }
}