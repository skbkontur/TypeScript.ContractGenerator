using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class ListTypeBuildingContext : ITypeBuildingContext
    {
        public ListTypeBuildingContext(Type listType)
        {
            itemType = listType.GetGenericArguments()[0];
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
            return new FlowTypeArrayType(itemFlowType);
        }

        private readonly Type itemType;
    }
}