using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypeGenerator
    {
        ITypeBuildingContext ResolveType(Type type);
        FlowTypeType BuildAndImportType(FlowTypeUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type);
    }
}