using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypeGenerator
    {
        ITypeBuildingContext ResolveType(ITypeInfo type);
        TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeInfo type, IPropertyInfo propertyInfo);
        TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, IAttributeProvider? customAttributeProvider, ITypeInfo type);

        ITypesProvider TypesProvider { get; }
        TypeScriptGenerationOptions Options { get; }
    }
}