using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
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

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo typeInfo, ITypeScriptUnitFactory unitFactory)
        {
            if (typeInfo.Equals(TypeInfo.FromType<MethodRootType>()))
                return new MethodTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            if (CollectionTypeBuildingContext.Accept(typeInfo))
                return new CollectionTypeBuildingContext(typeInfo);

            if (typeInfo.Equals(TypeInfo.FromType<TimeSpan>()))
                return new StringBuildingContext();

            if (typeInfo.IsAbstract)
                return new AbstractTypeBuildingContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);

            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var (isNullable, _) = TypeScriptGeneratorHelpers.ProcessNullable(propertyInfo, propertyInfo.PropertyType, typeGenerator.Options.NullabilityMode);

            if (!TryGetGetOnlyEnumPropertyValue(typeInfo, propertyInfo, out var value))
                return null;

            return new TypeScriptTypeMemberDeclaration
                {
                    Name = propertyInfo.Name.ToLowerCamelCase(),
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
                return new TypeScriptEnumValueType(typeGenerator.BuildAndImportType(unit, property, property.PropertyType), value);
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private static bool TryGetGetOnlyEnumPropertyValue(ITypeInfo typeInfo, IPropertyInfo propertyInfo, out string value)
        {
            var property = propertyInfo.Property;
            var type = typeInfo.Type;
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