using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(Type type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, Type type, ITypeScriptUnitFactory unitFactory)
        {
            if (type == typeof(MethodRootType))
                return new MethodTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), type);

            if (CollectionTypeBuildingContext.Accept(type))
                return new CollectionTypeBuildingContext(type);

            if (type == typeof(TimeSpan))
                return new StringBuildingContext();

            if (type.IsAbstract)
                return new AbstractTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), type);

            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, Type type, PropertyInfo property)
        {
            return null;
        }
    }
}