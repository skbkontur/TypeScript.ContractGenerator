using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(Type type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, Type type, IFlowTypeUnitFactory unitFactory)
        {
            if (CollectionTypeBuildingContext.Accept(type))
                return new CollectionTypeBuildingContext(type);

            if (type == typeof(TimeSpan))
                return new StringBuildingContext();

            return null;
        }
    }
}