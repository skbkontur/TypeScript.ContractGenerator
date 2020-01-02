using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class NullCustomTypeGenerator : ICustomTypeGenerator
    {
        [NotNull]
        public string GetTypeLocation([NotNull] ITypeInfo type)
        {
            return "";
        }

        [CanBeNull]
        public ITypeBuildingContext ResolveType([NotNull] string initialUnitPath, [NotNull] ITypeInfo type, [NotNull] ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        [CanBeNull]
        public TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] ITypeGenerator typeGenerator, [NotNull] ITypeInfo type, [NotNull] IPropertyInfo property)
        {
            return null;
        }
    }
}