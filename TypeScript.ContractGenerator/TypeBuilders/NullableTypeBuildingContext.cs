using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{//todo
    public class NullableTypeBuildingContext : ITypeBuildingContext
    {
        public NullableTypeBuildingContext(Type nullableUnderlyingType, bool useGlobalNullable)
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

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemTypeScriptType = typeGenerator.ResolveType(itemType).ReferenceFrom(targetUnit, typeGenerator);
            return useGlobalNullable
                       ? (TypeScriptType)new TypeScriptNullableType(itemTypeScriptType)
                       : new TypeScriptOrNullType(itemTypeScriptType);
        }

        private readonly Type itemType;
        private readonly bool useGlobalNullable;
    }
}