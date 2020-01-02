using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IPropertyInfo
    {
        PropertyInfo Property { get; }
        
        string Name { get; }
        ITypeInfo PropertyType { get; }
    }
}