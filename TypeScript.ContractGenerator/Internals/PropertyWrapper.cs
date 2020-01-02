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
        public ITypeInfo PropertyType => new TypeWrapper(Property.PropertyType);
    }
}