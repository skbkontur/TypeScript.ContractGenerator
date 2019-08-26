# Changelog

## v1.8 - 2019.08.26
- Add `RequiredAttribute` support
- Check interfaces' nullability
- Custom property resolving setup via fluent configuration
- Correct attributes retrieving for overriden properties
- Add `TypeScriptArrowFunction` to CodeDom
- Move `ResolveProperty` to `ITypeGenerator`
- Remove `ContractGeneratorInferValue` attribute
- Add `DerivedTypesUnionBuildingContext`
- Add `Upload` method to api type building context

## v1.7 - 2019.07.22
- Fix nullability issues in generic types
- Add ItemNotNull/ItemCanBeNull support
- Add `ApiControllerTypeBuildingContextBase`, add `ApiControllerTypeBuildingContext` example for asp net core
- Add CustomGenerator builder, add `RedirectToTypeBuildingContext`
- Update dependencies

## v1.6 - 2019.04.23
- Fix cyclic dependency bug when generating code for child types
- Pass `TypeScriptUnit` to `ResolveProperty` for `BuildAndImport` availability
- Add `NullabilityMode` option
- Add more code generation classes to CodeDom

## v1.5 - 2019.03.14
- Add `ContractGeneratorIgnore` and `ContractGeneratorInferValue` attributes that can be applied to properties
- Add `ResolveProperty` method to `ICustomTypeGenerator` for property customization

## v1.4 - 2019.03.08
- `//tslint:disable` is placed before codegen marker
- Global rename: `FlowType => TypeScript`
- Add support for enum properties with user-defined getter (see `GenerateEnumWithConstGetterTest` for details)

## v1.3 - 2019.02.22
- Correctly generate built-in types
- Add support for `List<T>` and `Dictionary<TKey, TValue>`

## v1.2 - 2019.02.21
- Add GlobalNullable generation option: if `true`, type `Nullable<T>` from global namespace will be used
- Fixed invalid Nullable<T> generation
- Use `Array<null | T>` instead of `Nullable<T>[]` if GlobalNullable option is set to `false` 

## v1.1 - 2019.02.18
- TypeScript files generated with '.ts' extension
- Generation options: EnumGenerationMode: FixedStringsAndDictionary | TypeScriptEnum, EnableOptionalProperties, EnableExplicitNullability
- IRootTypes provider interface (preparation to build in executable generator)
- Add internal JetBrains.Annotations
- Typos fixes at ITypeBuildingContext: IDefinitionBuilded -> IsDefinitionBuilt, BuildDefiniion -> BuildDefinition

## v1.0 - 2019.01.30
- Support .NET Standard 2.0.
- Switch to SDK-style project format and dotnet core build tooling.
- Use [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) to automate generation of assembly 
  and nuget package versions.
- Fix incorrect camelCase property names generation.
- Add tests.
