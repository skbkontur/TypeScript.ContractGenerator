using System;

using SKBKontur.Catalogue.FlowType.CodeDom;
using SKBKontur.Catalogue.FlowType.ContractGenerator;
using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.CustomTypeGenerators
{
    public class ListTypeBuildingContext : ITypeBuildingContext
    {
        public ListTypeBuildingContext(Type listType)
        {
            itemType = listType.GetGenericArguments()[0];
        }

        public bool IsDefinitionBuilded => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefiniion(ITypeGenerator typeGenerator)
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