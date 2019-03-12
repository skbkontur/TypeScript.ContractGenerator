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
        
        TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type);

        [NotNull]
        TypeScriptGenerationOptions Options { get; }
    }
}