using System;

using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        string GetTypeLocation(Type type);
        ITypeBuildingContext ResolveType(string initialUnitPath, Type type, IFlowTypeUnitFactory unitFactory);
    }
}