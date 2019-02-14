using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext(Type elementType)
        {
            this.elementType = elementType;
        }

        public bool IsDefinitionBuilt { get { return true; } }

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
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