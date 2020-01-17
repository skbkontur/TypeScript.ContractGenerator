using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IMethodInfo : IAttributeProvider
    {
        MethodInfo? Method { get; }

        string Name { get; }
        ITypeInfo ReturnType { get; }

        IParameterInfo[] GetParameters();
    }
}