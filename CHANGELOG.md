# Changelog
## v2.0.131 - 2023.05.09
- Use net7.0 in `TypeScript.ContractGenerator.Cli`
- Add `ApiControllerTypeBuildingContext`
- Update AspNetCoreExample to use latest web api template

## v2.0.126 - 2023.05.03
- Update dependencies
- Test against net7.0 tfm

## v2.0.121 - 2023.02.01
- Add `CustomContentMarker` option for custom header text in generated files
- Switch to `github-actions` for tests and releases

## v2.0.114 - 2022.11.18
- Add DateTimeOffset support

## v2.0.110 - 2022.08.11
- Add Patch method to ApiControllerTypeBuildingContextBase

## v2.0.106 - 2021.12.03
- Fix attributes data extraction

## v2.0.105 - 2021.11.11
- Use net6.0 in cli tool
- Update Roslyn dependencies

## v2.0.104 - 2021.10.28
- treat long type as number in javascript according to serializers' behavior (e.g. JSON.NET & Netwonsoft.JSON)

## v2.0.98 - 2021.09.01
- Added types in variable and constant definition

## v2.0.94 - 2021.03.11
- Use net5.0 in cli tool
- Add FrameworkReference to `Microsoft.AspNetCore.App` in cli tool
- Update dependencies

## v2.0.92 - 2021.03.10
- Fix Roslyn custom type generator compilation in net5.0

## v2.0.81 - 2020.06.22
- Remove deprecated options (See [MIGRATION](MIGRATION.md))
- Use abstractions instead of reflection types in public api (See [MIGRATION](MIGRATION.md))
- Fix nullability issues with Nullable Reference Types
- Add Roslyn support
- Add dotnet tool

## v1.10.3 - 2020.05.28
- Fix eslint-ignore comment
- Add public modifier to function definition
- Add EnableExplicitNullability obsoletion warning
- Update deps

## v1.10 - 2020.01.10
- Fix type definition duplicates for custom generic type building context 
- Add obsoletion warnings for `JavaScriptTypeChecker.Flow`, `EnumGenerationMode.FixedStringsAndDictionary` and `TypeScriptGenerationOptions.Pluralize`
- Use `EnumGenerationMode.TypeScriptEnum` and `LinterDisableMode.EsLint` in default options

## v1.9 - 2019.12.31
- Add nullable reference support, it can be enabled with `NullabilityMode.NullableReference` option

## v1.8.31 - 2019.11.25
- Use [SourceLink](https://github.com/dotnet/sourcelink) to help ReSharper decompiler show actual code.

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
- Add `virtual GenerateCustomBody` into `ApiControllerTypeBuildingContextBase` for request body building customization

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
