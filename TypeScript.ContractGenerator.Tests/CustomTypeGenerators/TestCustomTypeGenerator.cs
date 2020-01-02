using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomTypeGenerator : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo typeInfo, ITypeScriptUnitFactory unitFactory)
        {
            var type = typeInfo.Type;
            if (type == typeof(MethodRootType))
                return new MethodTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            if (CollectionTypeBuildingContext.Accept(type))
                return new CollectionTypeBuildingContext(typeInfo);

            if (type == typeof(TimeSpan))
                return new StringBuildingContext();

            if (type.IsAbstract)
                return new AbstractTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var type = typeInfo.Type;
            var property = propertyInfo.Property;
            var (isNullable, _) = TypeScriptGeneratorHelpers.ProcessNullable(property, property.PropertyType, typeGenerator.Options.NullabilityMode);

            if (!TryGetGetOnlyEnumPropertyValue(type, property, out var value))
                return null;

            return new TypeScriptTypeMemberDeclaration
                {
                    Name = property.Name.ToLowerCamelCase(),
                    Optional = isNullable && typeGenerator.Options.EnableOptionalProperties,
                    Type = GetConstEnumType(typeGenerator, unit, propertyInfo, value),
                };
        }

        private static TypeScriptType GetConstEnumType(ITypeGenerator typeGenerator, TypeScriptUnit unit, IPropertyInfo property, string value)
        {
            switch (typeGenerator.Options.EnumGenerationMode)
            {
            case EnumGenerationMode.FixedStringsAndDictionary:
                return new TypeScriptStringLiteralType(value);
            case EnumGenerationMode.TypeScriptEnum:
                return new TypeScriptEnumValueType(typeGenerator.BuildAndImportType(unit, property.Property, property.PropertyType), value);
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private static bool TryGetGetOnlyEnumPropertyValue(Type type, PropertyInfo property, out string value)
        {
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