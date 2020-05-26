using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeGenerator typeGenerator, ITypeInfo typeInfo, ITypeScriptUnitFactory unitFactory)
        {
            if (typeInfo.Equals(TypeInfo.From<MethodRootType>()))
                return new MethodTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            if (CollectionTypeBuildingContext.Accept(typeInfo))
                return new CollectionTypeBuildingContext(typeInfo);

            if (typeInfo.Equals(TypeInfo.From<TimeSpan>()))
                return new StringBuildingContext(typeInfo);

            if (typeInfo.IsAbstract)
                return new AbstractTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            if (!TryGetGetOnlyEnumPropertyValue(typeInfo, propertyInfo, out var value))
                return null;

            return new TypeScriptTypeMemberDeclaration
                {
                    Name = propertyInfo.Name.ToLowerCamelCase(),
                    Optional = false,
                    Type = GetConstEnumType(typeGenerator, unit, propertyInfo, value),
                };
        }

        private static TypeScriptType GetConstEnumType(ITypeGenerator typeGenerator, TypeScriptUnit unit, IPropertyInfo property, string value)
        {
            return new TypeScriptEnumValueType(typeGenerator.BuildAndImportType(unit, property.PropertyType), value);
        }

        private static bool TryGetGetOnlyEnumPropertyValue(ITypeInfo typeInfo, IPropertyInfo propertyInfo, out string value)
        {
            var property = ((PropertyWrapper)propertyInfo).Property;
            var type = ((TypeInfo)typeInfo).Type;
            var hasDefaultConstructor = type.GetConstructors().Any(x => x.GetParameters().Length == 0);
            var hasInferAttribute = property.GetCustomAttributes<InferValueAttribute>(true).Any();
            if (!property.PropertyType.IsEnum || property.CanWrite || !hasDefaultConstructor || !hasInferAttribute)
            {
                value = null;
                return false;
            }
            value = property.GetMethod.Invoke(Activator.CreateInstance(type), null).ToString();
            return true;
        }
    }
}