using System;
using System.Collections.Generic;
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
        public TypeScriptGenerator(TypeScriptGenerationOptions options, ICustomTypeGenerator customTypeGenerator, IRootTypesProvider typesProvider)
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
            BuildAllDefinitions();
            return typeUnitFactory.Units;
        }

        public void GenerateFiles(string targetPath)
        {
            BuildAllDefinitions();
            FilesGenerator.GenerateFiles(targetPath, typeUnitFactory, Options.LinterDisableMode, Options.CustomContentMarker);
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

            var typeScriptType = this.BuildAndImportType(unit, propertyInfo.PropertyType);
            return new TypeScriptTypeMemberDeclaration
                {
                    Name = propertyInfo.Name.ToLowerCamelCase(),
                    Optional = typeScriptType is INullabilityWrapperType && Options.EnableOptionalProperties,
                    Type = typeScriptType,
                };
        }

        private ITypeBuildingContext GetTypeBuildingContext(string typeLocation, ITypeInfo typeInfo)
        {
            if (BuiltinTypeBuildingContext.Accept(typeInfo))
                return new BuiltinTypeBuildingContext(typeInfo);

            if (ArrayTypeBuildingContext.Accept(typeInfo))
                return new ArrayTypeBuildingContext(typeInfo);

            if (DictionaryTypeBuildingContext.Accept(typeInfo))
                return new DictionaryTypeBuildingContext(typeInfo);

            if (TaskTypeBuildingContext.Accept(typeInfo))
                return new TaskTypeBuildingContext(typeInfo);

            if (NullableTypeBuildingContext.Accept(typeInfo))
                return new NullableTypeBuildingContext(typeInfo);

            if (typeInfo.IsEnum)
                return new TypeScriptEnumTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), typeInfo);

            if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
                return new GenericTypeTypeBuildingContext(typeInfo);

            if (typeInfo.IsGenericParameter)
                return new GenericParameterTypeBuildingContext(typeInfo);

            if (typeInfo.IsGenericTypeDefinition)
                return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), typeInfo);

            return new CustomTypeTypeBuildingContext(typeUnitFactory.GetOrCreateTypeUnit(typeLocation), typeInfo);
        }

        public TypeScriptGenerationOptions Options { get; }
        public IRootTypesProvider TypesProvider { get; }

        private readonly ITypeInfo[] rootTypes;
        private readonly DefaultTypeScriptGeneratorOutput typeUnitFactory;
        private readonly ICustomTypeGenerator customTypeGenerator;
        private readonly Dictionary<ITypeInfo, ITypeBuildingContext> typeDeclarations;
    }
}