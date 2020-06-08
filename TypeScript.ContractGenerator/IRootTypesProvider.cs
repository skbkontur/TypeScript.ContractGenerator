using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IRootTypesProvider
    {
        ITypeInfo[] GetRootTypes();
    }
}