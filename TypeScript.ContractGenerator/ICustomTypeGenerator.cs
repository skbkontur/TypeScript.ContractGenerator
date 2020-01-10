using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        string GetTypeLocation(ITypeInfo type);
        ITypeBuildingContext? ResolveType(string initialUnitPath, ITypeInfo type, ITypeScriptUnitFactory unitFactory);
        TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property);
    }
}