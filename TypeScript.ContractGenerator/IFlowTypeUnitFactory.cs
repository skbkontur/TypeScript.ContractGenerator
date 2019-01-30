namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IFlowTypeUnitFactory
    {
        FlowTypeUnit GetOrCreateTypeUnit(string path);
    }
}