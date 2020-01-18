namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IMethodInfo : IAttributeProvider
    {
        string Name { get; }
        ITypeInfo ReturnType { get; }
        ITypeInfo? DeclaringType { get; }

        IParameterInfo[] GetParameters();
    }
}