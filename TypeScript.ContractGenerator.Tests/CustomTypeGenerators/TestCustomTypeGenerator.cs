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
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return new ListTypeBuildingContext(type);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return new DictionaryTypeBuildingContext(type);
            if (type == typeof(Guid))
                return new StringBuildingContext();

            return null;
        }
    }
}