using System;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypeGenerator
    {
        [NotNull]
        ITypeBuildingContext ResolveType([NotNull] Type type);

        [CanBeNull]
        TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] Type type, [NotNull] PropertyInfo propertyInfo);

        [NotNull]
        TypeScriptType BuildAndImportType([NotNull] TypeScriptUnit targetUnit, [CanBeNull] ICustomAttributeProvider customAttributeProvider, [NotNull] Type type);

        [NotNull]
        TypeScriptGenerationOptions Options { get; }
    }
}