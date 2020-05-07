namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IParameterInfo : IAttributeProvider
    {
        string Name { get; }
        ITypeInfo ParameterType { get; }
        IMethodInfo Method { get; }
    }
}