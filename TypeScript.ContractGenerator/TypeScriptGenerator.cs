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
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.customTypeGenerator = customTypeGenerator ?? throw new ArgumentNullException(nameof(customTypeGenerator));
            rootTypes = rootTypesProvider?.GetRootTypes() ?? throw new ArgumentNullException(nameof(rootTypesProvider));
            typeUnitFactory = new DefaultTypeScriptGeneratorOutput();
            typeDeclarations = new Dictionary<Type, ITypeBuildingContext>();
        }

        public TypeScriptUnit[] Generate()
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
            return typeUnitFactory.Units;
        }

        public void GenerateFiles(string targetPath, JavaScriptTypeChecker javaScriptTypeChecker)
        {
            ValidateOptions(javaScriptTypeChecker, options);

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
        private static void ValidateOptions(JavaScriptTypeChecker javaScriptTypeChecker, TypeScriptGenerationOptions flowTypeGenerationOptions)
        {
            if (javaScriptTypeChecker == JavaScriptTypeChecker.Flow && flowTypeGenerationOptions.EnumGenerationMode == EnumGenerationMode.TypeScriptEnum)
                throw new ArgumentException("Flow is not compatible with TypeScript enums");
        }

        private void RequestTypeBuild(Type type)
        {
            ResolveType(type);
        }

        public ITypeBuildingContext ResolveType(Type type)
        {
            if (typeDeclarations.ContainsKey(type))
            {
                return typeDeclarations[type];
            }
            var typeLocation = customTypeGenerator.GetTypeLocation(type);
            var typeBuildingContext = customTypeGenerator.ResolveType(typeLocation, type, typeUnitFactory) ?? GetTypeBuildingContext(typeLocation, type);
            typeBuildingContext.Initialize(this);
            typeDeclarations.Add(type, typeBuildingContext);
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
                return options.EnumGenerationMode == EnumGenerationMode.FixedStringsAndDictionary
                           ? (ITypeBuildingContext)new FixedStringsAndDictionaryTypeBuildingContext(targetUnit, type)
                           : new TypeScriptEnumTypeBuildingContext(targetUnit, type);
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GenericTypeArguments.Single();
                if (options.EnableExplicitNullability)
                    return new NullableTypeBuildingContext(underlyingType, options.UseGlobalNullable);
                return GetTypeBuildingContext(typeLocation, underlyingType);
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                return new GenericTypeTypeBuildingContext(type);

            if (type.IsGenericParameter)
                return new GenericParameterTypeBuildingContext(type);

            if (type.IsGenericTypeDefinition)
                return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type, options);

            return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), type, options);
        }

        public TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, ICustomAttributeProvider attributeProvider, Type type)
        {
            var (isNullable, resultType) = TypeScriptGeneratorHelpers.ProcessNullable(attributeProvider, type);
            var result = GetTypeScriptType(targetUnit, resultType);
            if (isNullable && options.EnableExplicitNullability)
                result = new TypeScriptNullableType(result);
            return result;
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

        private readonly TypeScriptGenerationOptions options;
        private readonly Type[] rootTypes;
        private readonly DefaultTypeScriptGeneratorOutput typeUnitFactory;
        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly Dictionary<Type, ITypeBuildingContext> typeDeclarations;
    }
}