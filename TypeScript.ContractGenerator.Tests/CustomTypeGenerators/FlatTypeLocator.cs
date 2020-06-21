using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class FlatTypeLocator : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return type.Name;
        }

        public ITypeBuildingContext? ResolveType(string initialUnitPath, ITypeGenerator typeGenerator, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property)
        {
            return null;
        }
    }
}