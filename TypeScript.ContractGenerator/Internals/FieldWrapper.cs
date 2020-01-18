using System.Linq;
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
        public ITypeInfo FieldType => TypeInfo.From(Field.FieldType);
        public ITypeInfo? DeclaringType => TypeInfo.From(Field.DeclaringType);

        public object[] GetCustomAttributes(bool inherit)
        {
            return Field.GetCustomAttributes(inherit);
        }

        public bool IsNameDefined(string name)
        {
            return Field.GetCustomAttributes(true).Any(x => x.GetType().Name == name);
        }
    }
}