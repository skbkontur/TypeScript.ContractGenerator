using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class NullableTypeBuildingContext : ITypeBuildingContext
    {
        public NullableTypeBuildingContext(ITypeInfo nullableUnderlyingType, bool useGlobalNullable)
        {
            itemType = nullableUnderlyingType;
            this.useGlobalNullable = useGlobalNullable;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider? attributeProvider)
        {
            var itemTypeScriptType = typeGenerator.ResolveType(itemType).ReferenceFrom(targetUnit, typeGenerator, null);
            return useGlobalNullable
                       ? (TypeScriptType)new TypeScriptNullableType(itemTypeScriptType)
                       : new TypeScriptOrNullType(itemTypeScriptType);
        }

        private readonly ITypeInfo itemType;
        private readonly bool useGlobalNullable;
    }
}