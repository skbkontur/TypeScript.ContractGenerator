using System;
using System.Reflection;

using SKBKontur.Catalogue.FlowType.CodeDom;
using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator
{
    public interface ITypeGenerator
    {
        ITypeBuildingContext ResolveType(Type type);
        FlowTypeType BuildAndImportType(FlowTypeUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type);
    }
}