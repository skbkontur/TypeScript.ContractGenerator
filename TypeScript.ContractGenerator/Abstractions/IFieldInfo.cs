namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IFieldInfo : IAttributeProvider
    {
        string Name { get; }
        ITypeInfo FieldType { get; }
        ITypeInfo? DeclaringType { get; }
    }
}