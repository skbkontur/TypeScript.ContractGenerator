namespace TypeScript.ContractGenerator
{
    public interface IFlowTypeUnitFactory
    {
        FlowTypeUnit GetOrCreateTypeUnit(string path);
    }
}