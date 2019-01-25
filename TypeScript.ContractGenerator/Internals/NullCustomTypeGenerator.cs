using System;

using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.Internals
{
    internal class NullCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(Type type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, Type type, IFlowTypeUnitFactory unitFactory)
        {
            return null;
        }
    }
}