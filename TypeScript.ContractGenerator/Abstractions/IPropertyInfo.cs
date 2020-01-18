namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IPropertyInfo : IAttributeProvider
    {
        string Name { get; }
        ITypeInfo PropertyType { get; }
        ITypeInfo? DeclaringType { get; }
    }
}