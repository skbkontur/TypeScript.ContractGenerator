using System;

using SKBKontur.Catalogue.FlowType.CodeDom;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext(Type elementType)
        {
            this.elementType = elementType;
        }

        public bool IsDefinitionBuilded { get { return true; } }

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefiniion(ITypeGenerator typeGenerator)
        {
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator);
            return new FlowTypeArrayType(itemType);
        }

        private readonly Type elementType;
    }
}