# Changelog

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