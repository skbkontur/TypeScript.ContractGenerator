using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ICustomTypeGenerator
    {
        string GetTypeLocation(Type type);
        ITypeBuildingContext ResolveType(string initialUnitPath, Type type, ITypeScriptUnitFactory unitFactory);
        TypeScriptTypeMemberDeclaration ResolveProperty(ITypeGenerator typeGenerator, Type type, PropertyInfo property);
    }
}