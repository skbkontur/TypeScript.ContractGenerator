namespace SKBKontur.Catalogue.FlowType.ContractGenerator
{
    public interface IFlowTypeUnitFactory
    {
        FlowTypeUnit GetOrCreateTypeUnit(string path);
    }
}