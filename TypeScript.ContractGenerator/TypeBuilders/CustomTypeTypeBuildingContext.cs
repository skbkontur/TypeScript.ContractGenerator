using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Attributes;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CustomTypeTypeBuildingContext : TypeBuildingContext
    {
        public CustomTypeTypeBuildingContext([NotNull] TypeScriptUnit unit, [NotNull] Type type, [NotNull] ICustomTypeGenerator customTypeGenerator, [NotNull] TypeScriptGenerationOptions options)
            : base(unit, type)
        {
            this.customTypeGenerator = customTypeGenerator;
            this.options = options;
        }

        public override bool IsDefinitionBuilt => Declaration.Definition != null;

        private TypeScriptTypeDeclaration CreateComplexTypeScriptDeclarationWithoutDefinition(Type type)
        {
            var result = new TypeScriptTypeDeclaration
                {
                    Name = type.IsGenericType ? new Regex("`.*$").Replace(type.GetGenericTypeDefinition().Name, "") : type.Name,
                    Definition = null,
                    GenericTypeArguments = Type.IsGenericTypeDefinition ? Type.GetGenericArguments().Select(x => x.Name).ToArray() : null
                };
            return result;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            if (Type.BaseType != typeof(object) && Type.BaseType != typeof(ValueType) && Type.BaseType != typeof(MarshalByRefObject) && Type.BaseType != null)
            {
                typeGenerator.ResolveType(Type.BaseType);
            }
            Declaration = CreateComplexTypeScriptDeclarationWithoutDefinition(Type);
            Unit.Body.Add(new TypeScriptExportTypeStatement {Declaration = Declaration});
        }

        public override void BuildDefinition(ITypeGenerator typeGenerator)
        {
            Declaration.Definition = CreateComplexTypeScriptDefinition(typeGenerator);
        }

        protected virtual TypeScriptTypeDefintion CreateComplexTypeScriptDefinition(ITypeGenerator typeGenerator)
        {
            var result = new TypeScriptTypeDefintion();
            var properties = CreateTypeProperties(Type);
            foreach (var property in properties)
            {
                var customMemberDeclaration = customTypeGenerator.ResolveProperty(Unit, typeGenerator, Type, property);
                if (customMemberDeclaration != null)
                {
                    result.Members.Add(customMemberDeclaration);
                    continue;
                }

                if (property.GetCustomAttributes<ContractGeneratorIgnoreAttribute>().Any())
                    continue;

                var (isNullable, type) = TypeScriptGeneratorHelpers.ProcessNullable(property, property.PropertyType);

                if (TryGetGetOnlyEnumPropertyValue(property, out var value))
                {
                    result.Members.Add(new TypeScriptTypeMemberDeclaration
                        {
                            Name = BuildPropertyName(property.Name),
                            Optional = isNullable && options.EnableOptionalProperties,
                            Type = GetConstEnumType(typeGenerator, property, value),
                        });
                }
                else
                {
                    result.Members.Add(new TypeScriptTypeMemberDeclaration
                        {
                            Name = BuildPropertyName(property.Name),
                            Optional = isNullable && options.EnableOptionalProperties,
                            Type = GetMaybeNullableComplexType(typeGenerator, type, property, isNullable),
                        });
                }
            }
            return result;
        }

        private TypeScriptType GetConstEnumType(ITypeGenerator typeGenerator, PropertyInfo property, string value)
        {
            switch (options.EnumGenerationMode)
            {
            case EnumGenerationMode.FixedStringsAndDictionary:
                return new TypeScriptStringLiteralType(value);
            case EnumGenerationMode.TypeScriptEnum:
                return new TypeScriptEnumValueType(typeGenerator.BuildAndImportType(Unit, property, property.PropertyType), value);
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryGetGetOnlyEnumPropertyValue(PropertyInfo property, out string value)
        {
            var hasDefaultConstructor = Type.GetConstructors().Any(x => x.GetParameters().Length == 0);
            var hasInferAttribute = property.GetCustomAttributes<ContractGeneratorInferValueAttribute>(true).Any();
            if (!property.PropertyType.IsEnum || property.CanWrite || !hasDefaultConstructor || !hasInferAttribute)
            {
                value = null;
                return false;
            }
            value = property.GetMethod.Invoke(Activator.CreateInstance(Type), null).ToString();
            return true;
        }

        private TypeScriptType GetMaybeNullableComplexType(ITypeGenerator typeGenerator, Type type, PropertyInfo property, bool isNullable)
        {
            var propertyType = typeGenerator.BuildAndImportType(Unit, null, type);

            if (property.PropertyType.IsGenericParameter)
                return new TypeScriptTypeReference(property.PropertyType.Name);

            if (isNullable && options.EnableExplicitNullability && !options.UseGlobalNullable)
                return new TypeScriptOrNullType(propertyType);

            if (isNullable && options.EnableExplicitNullability && options.UseGlobalNullable)
                return new TypeScriptNullableType(propertyType);

            return propertyType;
        }

        private static string BuildPropertyName(string propertyName)
        {
            return propertyName.ToLowerCamelCase();
        }

        protected virtual PropertyInfo[] CreateTypeProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly TypeScriptGenerationOptions options;
    }
}