using TypeScript.CodeDom;
using TypeScript.ContractGenerator.TypeBuilders;

namespace TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class StringBuildingContext : ITypeBuildingContext
    {
        public bool IsDefinitionBuilded => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefiniion(ITypeGenerator typeGenerator)
        {
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return new FlowTypeBuildInType("string");
        }
    }
}