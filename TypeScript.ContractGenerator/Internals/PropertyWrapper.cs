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
        public ITypeInfo PropertyType => TypeInfo.From(Property.PropertyType).WithMemberInfo(this);
        public ITypeInfo? DeclaringType => TypeInfo.From(Property.DeclaringType);

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return Property.GetAttributes(inherit);
        }
    }
}