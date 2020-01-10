using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class NullCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return "";
        }

        public ITypeBuildingContext? ResolveType(string initialUnitPath, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property)
        {
            return null;
        }
    }
}