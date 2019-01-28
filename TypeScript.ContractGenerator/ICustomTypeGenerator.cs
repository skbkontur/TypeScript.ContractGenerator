using System;
using TypeScript.ContractGenerator.TypeBuilders;

namespace TypeScript.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        string GetTypeLocation(Type type);
        ITypeBuildingContext ResolveType(string initialUnitPath, Type type, IFlowTypeUnitFactory unitFactory);
    }
}