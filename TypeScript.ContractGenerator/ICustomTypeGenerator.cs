using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        [NotNull]
        string GetTypeLocation([NotNull] ITypeInfo type);

        [CanBeNull]
        ITypeBuildingContext ResolveType([NotNull] string initialUnitPath, [NotNull] ITypeInfo type, [NotNull] ITypeScriptUnitFactory unitFactory);

        [CanBeNull]
        TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] ITypeGenerator typeGenerator, [NotNull] ITypeInfo type, [NotNull] IPropertyInfo property);
    }
}