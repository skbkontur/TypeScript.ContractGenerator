using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypesProvider
    {
        [NotNull]
        ITypeInfo[] GetRootTypes();

        ITypeInfo[] GetAssemblyTypes(ITypeInfo type);
    }
}