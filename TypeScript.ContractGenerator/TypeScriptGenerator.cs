using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
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
        public TypeScriptGenerator(TypeScriptGenerationOptions options, ICustomTypeGenerator customTypeGenerator, ITypesProvider typesProvider)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            TypesProvider = typesProvider ?? throw new ArgumentNullException(nameof(typesProvider));
            this.customTypeGenerator = customTypeGenerator ?? throw new ArgumentNullException(nameof(customTypeGenerator));
            rootTypes = typesProvider?.GetRootTypes() ?? throw new ArgumentNullException(nameof(typesProvider));
            typeUnitFactory = new DefaultTypeScriptGeneratorOutput();
            typeDeclarations = new Dictionary<ITypeInfo, ITypeBuildingContext>();
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
        private static void ValidateOptions(TypeScriptGenerationOptions options, JavaScriptTypeChecker? javaScriptTypeChecker = null)
        {
            if (javaScriptTypeChecker == JavaScriptTypeChecker.Flow && options.EnumGenerationMode == EnumGenerationMode.TypeScriptEnum)
                throw new ArgumentException("Flow is not compatible with TypeScript enums");

            const string enumName = "Enum";
            if (options.Pluralize == null || string.IsNullOrEmpty(options.Pluralize(enumName)) || enumName == options.Pluralize(enumName))
                throw new ArgumentException("Invalid Pluralize function: Pluralize cannot return null, empty string or unchanged argument");
        }

        private void RequestTypeBuild(ITypeInfo type)
        {
            ResolveType(type);
        }

        public ITypeBuildingContext ResolveType(ITypeInfo typeInfo)
        {
            if (typeDeclarations.ContainsKey(typeInfo))
                return typeDeclarations[typeInfo];
            var typeLocation = customTypeGenerator.GetTypeLocation(typeInfo);
            var typeBuildingContext = customTypeGenerator.ResolveType(typeLocation, this, typeInfo, typeUnitFactory)
                                      ?? GetTypeBuildingContext(typeLocation, typeInfo);
            typeDeclarations.Add(typeInfo, typeBuildingContext);
            typeBuildingContext.Initialize(this);
            return typeBuildingContext;
        }

        public TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var customMemberDeclaration = customTypeGenerator.ResolveProperty(unit, this, typeInfo, propertyInfo);
            if (customMemberDeclaration != null)
                return customMemberDeclaration;

            if (propertyInfo.IsNameDefined(nameof(ContractGeneratorIgnoreAttribute)))
                return null;

            var (isNullable, trueType) = TypeScriptGeneratorHelpers.ProcessNullable(propertyInfo, propertyInfo.PropertyType, Options.NullabilityMode);
            return new TypeScriptTypeMemberDeclaration
                {
                    Name = propertyInfo.Name.ToLowerCamelCase(),
                    Optional = isNullable && Options.EnableOptionalProperties,
                    Type = GetMaybeNullableComplexType(unit, trueType, propertyInfo, isNullable),
                };
        }

        private TypeScriptType GetMaybeNullableComplexType(TypeScriptUnit unit, ITypeInfo type, IPropertyInfo property, bool isNullable)
        {
            var propertyType = BuildAndImportType(unit, property, type);
            if (property.PropertyType.IsGenericParameter)
                propertyType = new TypeScriptTypeReference(property.PropertyType.Name);

            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(propertyType, isNullable, Options);
        }

        private ITypeBuildingContext GetTypeBuildingContext(string typeLocation, ITypeInfo typeInfo)
        {
            if (BuiltinTypeBuildingContext.Accept(typeInfo))
                return new BuiltinTypeBuildingContext(typeInfo);

            if (ArrayTypeBuildingContext.Accept(typeInfo))
                return new ArrayTypeBuildingContext(typeInfo, Options);

            if (DictionaryTypeBuildingContext.Accept(typeInfo))
                return new DictionaryTypeBuildingContext(typeInfo, Options);

            if (typeInfo.IsEnum)
            {
                var targetUnit = typeUnitFactory.GetOrCreateTypeUnit(typeLocation);
                return Options.EnumGenerationMode == EnumGenerationMode.FixedStringsAndDictionary
                           ? (ITypeBuildingContext)new FixedStringsAndDictionaryTypeBuildingContext(targetUnit, typeInfo)
                           : new TypeScriptEnumTypeBuildingContext(targetUnit, typeInfo);
            }

            if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition && typeInfo.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Nullable<>))))
            {
                var underlyingType = typeInfo.GetGenericArguments().Single();
                if (Options.EnableExplicitNullability)
                    return new NullableTypeBuildingContext(underlyingType, Options.UseGlobalNullable);
                return GetTypeBuildingContext(typeLocation, underlyingType);
            }

            if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
                return new GenericTypeTypeBuildingContext(typeInfo, Options);

            if (typeInfo.IsGenericParameter)
                return new GenericParameterTypeBuildingContext(typeInfo);

            if (typeInfo.IsGenericTypeDefinition)
                return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), typeInfo);

            return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), typeInfo);
        }

        public TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, IAttributeProvider? attributeProvider, ITypeInfo typeInfo)
        {
            var (isNullable, resultType) = TypeScriptGeneratorHelpers.ProcessNullable(attributeProvider, typeInfo, Options.NullabilityMode);
            var targetType = GetTypeScriptType(targetUnit, resultType, attributeProvider);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, isNullable, Options);
        }

        private TypeScriptType GetTypeScriptType(TypeScriptUnit targetUnit, ITypeInfo typeInfo, IAttributeProvider? attributeProvider)
        {
            if (typeDeclarations.ContainsKey(typeInfo))
                return typeDeclarations[typeInfo].ReferenceFrom(targetUnit, this, attributeProvider);
            if (typeInfo.IsGenericTypeDefinition && typeInfo.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Nullable<>))))
                return new TypeScriptNullableType(GetTypeScriptType(targetUnit, typeInfo.GetGenericArguments()[0], null));
            return ResolveType(typeInfo).ReferenceFrom(targetUnit, this, attributeProvider);
        }

        public TypeScriptGenerationOptions Options { get; }
        public ITypesProvider TypesProvider { get; }

        private readonly ITypeInfo[] rootTypes;
        private readonly DefaultTypeScriptGeneratorOutput typeUnitFactory;
        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly Dictionary<ITypeInfo, ITypeBuildingContext> typeDeclarations;
    }
}