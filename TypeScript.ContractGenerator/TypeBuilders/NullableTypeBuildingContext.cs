using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
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

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemFlowType = typeGenerator.ResolveType(itemType).ReferenceFrom(targetUnit, typeGenerator);
            return useGlobalNullable
                       ? (FlowTypeType)new FlowTypeNullableType(itemFlowType)
                       : new FlowTypeOrNullType(itemFlowType);
        }

        private readonly Type itemType;
        private readonly bool useGlobalNullable;
    }
}