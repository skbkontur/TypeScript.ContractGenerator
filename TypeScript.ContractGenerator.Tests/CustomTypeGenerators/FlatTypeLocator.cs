using System;

using SKBKontur.Catalogue.FlowType.ContractGenerator;
using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.CustomTypeGenerators
{
    public class FlatTypeLocator : ICustomTypeGenerator
    {
        public string GetTypeLocation(Type type)
        {
            return type.Name;
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, Type type, IFlowTypeUnitFactory unitFactory)
        {
            return null;
        }
    }
}