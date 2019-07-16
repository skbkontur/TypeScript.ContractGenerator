using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
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
            typeDeclarations = new Dictionary<Type, ITypeBuildingContext>();
        }

        public TypeScriptUnit[] Generate()
        {
            ValidateOptions(Options);

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
            return typeUnitFactory.Units;
        }

        public void GenerateFiles(string targetPath, JavaScriptTypeChecker javaScriptTypeChecker)
        {
            ValidateOptions(Options, javaScriptTypeChecker);

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
            FilesGenerator.GenerateFiles(targetPath, typeUnitFactory, FilesGenerationContext.Create(javaScriptTypeChecker));
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
            if (typeDeclarations.ContainsKey(type))
            {
                return typeDeclarations[type];
            }
            var typeLocation = customTypeGenerator.GetTypeLocation(type);
            var typeBuildingContext = customTypeGenerator.ResolveType(typeLocation, type, typeUnitFactory) ?? GetTypeBuildingContext(typeLocation, type);
            typeDeclarations.Add(type, typeBuildingContext);
            typeBuildingContext.Initialize(this);
            return typeBuildingContext;
        }

        private ITypeBuildingContext GetTypeBuildingContext(string typeLocation, Type type)
        {
            if (BuildInTypeBuildingContext.Accept(type))
                return new BuildInTypeBuildingContext(type);

            if (ArrayTypeBuildingContext.Accept(type))
                return new ArrayTypeBuildingContext(type);

            if (DictionaryTypeBuildingContext.Accept(type))
                return new DictionaryTypeBuildingContext(type);

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
                return GetTypeBuildingContext(typeLocation, underlyingType);
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                return new GenericTypeTypeBuildingContext(type);

            if (type.IsGenericParameter)
                return new GenericParameterTypeBuildingContext(type);

            if (type.IsGenericTypeDefinition)
                return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type, customTypeGenerator, Options);

            return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type, customTypeGenerator, Options);
        }

        public TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type)
        {
            var (isNullable, resultType) = TypeScriptGeneratorHelpers.ProcessNullable(attributeProvider, type, Options.NullabilityMode);
            var targetType = GetTypeScriptType(targetUnit, resultType);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, isNullable, Options);
        }

        private TypeScriptType GetTypeScriptType(TypeScriptUnit targetUnit, Type type)
        {
            if (typeDeclarations.ContainsKey(type))
                return typeDeclarations[type].ReferenceFrom(targetUnit, this);
            if (type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return new TypeScriptNullableType(GetTypeScriptType(targetUnit, type.GetGenericArguments()[0]));
            var context = ResolveType(type);
            return context.ReferenceFrom(targetUnit, this);
        }

        [NotNull]
        public TypeScriptGenerationOptions Options { get; }

        private readonly Type[] rootTypes;
        private readonly DefaultTypeScriptGeneratorOutput typeUnitFactory;
        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly Dictionary<Type, ITypeBuildingContext> typeDeclarations;
    }
}