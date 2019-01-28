using TypeScript.CodeDom;

namespace TypeScript.ContractGenerator.TypeBuilders
{
    public interface ITypeBuildingContext
    {
        bool IsDefinitionBuilded { get; }
        void Initialize(ITypeGenerator typeGenerator);
        void BuildDefiniion(ITypeGenerator typeGenerator);
        FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator);
    }
}