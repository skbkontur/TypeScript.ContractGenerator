using SKBKontur.Catalogue.FlowType.CodeDom;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders
{
    public interface ITypeBuildingContext
    {
        bool IsDefinitionBuilded { get; }
        void Initialize(ITypeGenerator typeGenerator);
        void BuildDefiniion(ITypeGenerator typeGenerator);
        FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator);
    }
}