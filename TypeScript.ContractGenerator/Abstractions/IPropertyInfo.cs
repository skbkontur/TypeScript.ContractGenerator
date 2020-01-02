using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IPropertyInfo : IAttributeProvider
    {
        PropertyInfo Property { get; }
        
        string Name { get; }
        ITypeInfo PropertyType { get; }
    }
}