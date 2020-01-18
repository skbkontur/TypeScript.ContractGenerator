using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypesProvider
    {
        ITypeInfo[] GetRootTypes();
        ITypeInfo[] GetAssemblyTypes(ITypeInfo type);
    }
}