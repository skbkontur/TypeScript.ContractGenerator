using System;
using System.Collections.Generic;

using SKBKontur.Catalogue.FlowType.ContractGenerator;
using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.CustomTypeGenerators
{
    public class CustomTypeGenerator : ICustomTypeGenerator
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