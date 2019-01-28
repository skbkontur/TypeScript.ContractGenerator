using System;
using TypeScript.ContractGenerator.TypeBuilders;

namespace TypeScript.ContractGenerator.Tests.CustomTypeGenerators
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