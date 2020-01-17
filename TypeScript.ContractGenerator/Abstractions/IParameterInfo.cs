using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IParameterInfo : IAttributeProvider
    {
        ParameterInfo? Parameter { get; }

        string Name { get; }
        ITypeInfo ParameterType { get; }
    }
}