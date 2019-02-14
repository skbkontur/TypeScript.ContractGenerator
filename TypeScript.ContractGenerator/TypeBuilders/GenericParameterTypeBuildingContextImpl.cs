using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericParameterTypeBuildingContextImpl : ITypeBuildingContext
    {
        public GenericParameterTypeBuildingContextImpl(Type type)
        {
            this.type = type;
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
            return new FlowTypeTypeReference(type.Name);
        }

        private readonly Type type;
    }
}