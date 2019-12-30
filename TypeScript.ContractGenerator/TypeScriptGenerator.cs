using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Attributes;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypeScriptGenerator : ITypeGenerator
    {
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
        [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
        public TypeScriptGenerator([NotNull] TypeScriptGenerationOptions options, [NotNull] ICustomTypeGenerator customTypeGenerator, [NotNull] IRootTypesProvider rootTypesProvider)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            this.customTypeGenerator = customTypeGenerator ?? throw new ArgumentNullException(nameof(customTypeGenerator));
            rootTypes = rootTypesProvider?.GetRootTypes() ?? throw new ArgumentNullException(nameof(rootTypesProvider));
            typeUnitFactory = new DefaultTypeScriptGeneratorOutput();
            typeDeclarations = new Dictionary<TypeDeclarationKey, ITypeBuildingContext>();
        }

        public TypeScriptUnit[] Generate()
        {
            ValidateOptions(Options);
            BuildAllDefinitions();
            return typeUnitFactory.Units;
        }

        public void GenerateFiles(string targetPath, JavaScriptTypeChecker javaScriptTypeChecker)
        {
            ValidateOptions(Options, javaScriptTypeChecker);
            BuildAllDefinitions();
            FilesGenerator.GenerateFiles(targetPath, typeUnitFactory, FilesGenerationContext.Create(javaScriptTypeChecker, Options.LinterDisableMode));
        }

        private void BuildAllDefinitions()
        {
            foreach (var type in rootTypes)
                RequestTypeBuild(type);

            while (typeDeclarations.Values.Any(x => !x.IsDefinitionBuilt))
            {
                foreach (var currentType in typeDeclarations.ToArray())
                {
                    if (!currentType.Value.IsDefinitionBuilt)
                        currentType.Value.BuildDefinition(this);
                }
            }
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private static void ValidateOptions([NotNull] TypeScriptGenerationOptions options, JavaScriptTypeChecker? javaScriptTypeChecker = null)
        {
            if (javaScriptTypeChecker == JavaScriptTypeChecker.Flow && options.EnumGenerationMode == EnumGenerationMode.TypeScriptEnum)
                throw new ArgumentException("Flow is not compatible with TypeScript enums");

            const string enumName = "Enum";
            if (options.Pluralize == null || string.IsNullOrEmpty(options.Pluralize(enumName)) || enumName == options.Pluralize(enumName))
                throw new ArgumentException("Invalid Pluralize function: Pluralize cannot return null, empty string or unchanged argument");
        }

        private void RequestTypeBuild(Type type)
        {
            ResolveType(type);
        }

        [NotNull]
        public ITypeBuildingContext ResolveType([NotNull] Type type)
        {
            return ResolveType(type, customAttributeProvider : null);
        }

        [CanBeNull]
        public TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] Type type, [NotNull] PropertyInfo property)
        {
            var customMemberDeclaration = customTypeGenerator.ResolveProperty(unit, this, type, property);
            if (customMemberDeclaration != null)
                return customMemberDeclaration;

            if (property.GetCustomAttributes<ContractGeneratorIgnoreAttribute>().Any())
                return null;

            var (isNullable, trueType) = TypeScriptGeneratorHelpers.ProcessNullable(property, property.PropertyType, Options.NullabilityMode);
            return new TypeScriptTypeMemberDeclaration
                {
                    Name = property.Name.ToLowerCamelCase(),
                    Optional = isNullable && Options.EnableOptionalProperties,
                    Type = GetMaybeNullableComplexType(unit, trueType, property, isNullable),
                };
        }

        private TypeScriptType GetMaybeNullableComplexType(TypeScriptUnit unit, Type type, PropertyInfo property, bool isNullable)
        {
            var propertyType = BuildAndImportType(unit, property, type);
            if (property.PropertyType.IsGenericParameter)
                propertyType = new TypeScriptTypeReference(property.PropertyType.Name);

            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(propertyType, isNullable, Options);
        }

        [NotNull]
        private ITypeBuildingContext ResolveType([NotNull] Type type, [CanBeNull] ICustomAttributeProvider customAttributeProvider)
        {
            var typeDeclarationKey = new TypeDeclarationKey(type, customAttributeProvider);
            if (typeDeclarations.ContainsKey(typeDeclarationKey))
            {
                return typeDeclarations[typeDeclarationKey];
            }
            var typeLocation = customTypeGenerator.GetTypeLocation(type);
            var typeBuildingContext = customTypeGenerator.ResolveType(typeLocation, type, typeUnitFactory)
                                      ?? GetTypeBuildingContext(typeLocation, type, customAttributeProvider);
            typeDeclarations.Add(typeDeclarationKey, typeBuildingContext);
            typeBuildingContext.Initialize(this);
            return typeBuildingContext;
        }

        private ITypeBuildingContext GetTypeBuildingContext(string typeLocation, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            if (BuildInTypeBuildingContext.Accept(type))
                return new BuildInTypeBuildingContext(type);

            if (ArrayTypeBuildingContext.Accept(type))
                return new ArrayTypeBuildingContext(type, customAttributeProvider, Options);

            if (DictionaryTypeBuildingContext.Accept(type))
                return new DictionaryTypeBuildingContext(type, customAttributeProvider, Options);

            if (type.IsEnum)
            {
                var targetUnit = typeUnitFactory.GetOrCreateTypeUnit(typeLocation);
                return Options.EnumGenerationMode == EnumGenerationMode.FixedStringsAndDictionary
                           ? (ITypeBuildingContext)new FixedStringsAndDictionaryTypeBuildingContext(targetUnit, type)
                           : new TypeScriptEnumTypeBuildingContext(targetUnit, type);
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GenericTypeArguments.Single();
                if (Options.EnableExplicitNullability)
                    return new NullableTypeBuildingContext(underlyingType, Options.UseGlobalNullable);
                return GetTypeBuildingContext(typeLocation, underlyingType, underlyingType);
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                return new GenericTypeTypeBuildingContext(type);

            if (type.IsGenericParameter)
                return new GenericParameterTypeBuildingContext(type);

            if (type.IsGenericTypeDefinition)
                return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type);

            return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type);
        }

        [NotNull]
        public TypeScriptType BuildAndImportType([NotNull] TypeScriptUnit targetUnit, [CanBeNull] ICustomAttributeProvider customAttributeProvider, [NotNull] Type type)
        {
            var (isNullable, resultType) = TypeScriptGeneratorHelpers.ProcessNullable(customAttributeProvider, type, Options.NullabilityMode);
            var targetType = GetTypeScriptType(targetUnit, resultType, customAttributeProvider);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, isNullable, Options);
        }

        [NotNull]
        private TypeScriptType GetTypeScriptType([NotNull] TypeScriptUnit targetUnit, [NotNull] Type type, [CanBeNull] ICustomAttributeProvider customAttributeProvider)
        {
            customAttributeProvider = ArrayTypeBuildingContext.Accept(type) || DictionaryTypeBuildingContext.Accept(type) ? customAttributeProvider : null;
            var typeDeclarationKey = new TypeDeclarationKey(type, customAttributeProvider);
            if (typeDeclarations.ContainsKey(typeDeclarationKey))
                return typeDeclarations[typeDeclarationKey].ReferenceFrom(targetUnit, this);
            if (type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return new TypeScriptNullableType(GetTypeScriptType(targetUnit, type.GetGenericArguments()[0], null));
            var context = ResolveType(type, customAttributeProvider);
            return context.ReferenceFrom(targetUnit, this);
        }

        [NotNull]
        public TypeScriptGenerationOptions Options { get; }

        private readonly Type[] rootTypes;
        private readonly DefaultTypeScriptGeneratorOutput typeUnitFactory;
        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly Dictionary<TypeDeclarationKey, ITypeBuildingContext> typeDeclarations;
    }
}