using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class NullableTypeBuildingContext : ITypeBuildingContext
    {
        public NullableTypeBuildingContext(Type nullableType)
        {
            itemType = nullableType.GetGenericArguments()[0];
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
            return new FlowTypeNullableType(itemFlowType);
        }

        private readonly Type itemType;
    }
}