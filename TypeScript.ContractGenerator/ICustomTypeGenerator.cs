using System;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        [NotNull]
        string GetTypeLocation([NotNull] Type type);

        [CanBeNull]
        ITypeBuildingContext ResolveType([NotNull] string initialUnitPath, [NotNull] Type type, [NotNull] ITypeScriptUnitFactory unitFactory);

        [CanBeNull]
        TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] ITypeGenerator typeGenerator, [NotNull] Type type, [NotNull] PropertyInfo property);
    }
}