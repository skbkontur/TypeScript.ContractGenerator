using System;
using System.Reflection;
using TypeScript.CodeDom;
using TypeScript.ContractGenerator.TypeBuilders;

namespace TypeScript.ContractGenerator
{
    public interface ITypeGenerator
    {
        ITypeBuildingContext ResolveType(Type type);
        FlowTypeType BuildAndImportType(FlowTypeUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type);
    }
}