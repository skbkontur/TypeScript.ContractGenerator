using System;

using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
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