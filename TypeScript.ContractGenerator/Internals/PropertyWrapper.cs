using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class PropertyWrapper : IPropertyInfo
    {
        public PropertyWrapper(PropertyInfo property)
        {
            Property = property;
        }

        public PropertyInfo Property { get; }

        public string Name => Property.Name;
        public ITypeInfo PropertyType => TypeInfo.From(Property.PropertyType);

        public object[] GetCustomAttributes(bool inherit)
        {
            return Property.GetCustomAttributes(inherit);
        }

        public bool IsNameDefined(string name)
        {
            return Property.GetCustomAttributes(true).Any(x => x.GetType().Name == name);
        }
    }
}