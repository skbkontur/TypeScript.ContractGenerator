# TypeScript.ContractGenerator

A tool that can generate TypeScript types from C# classes

|              | Build Status
|--------------|:--------------:
| TypeScript.ContractGenerator | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator/)
| TypeScript.ContractGenerator.Roslyn | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.Roslyn.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator.Roslyn/)
| TypeScript.ContractGenerator.Cli | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.Cli.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator.Cli/)
| Build | [![Build status](https://ci.appveyor.com/api/projects/status/1x5x9gw0a7h12g38/branch/master?svg=true)](https://ci.appveyor.com/project/skbkontur/typescript-contractgenerator/branch/master)

## Release Notes

See [CHANGELOG](CHANGELOG.md).

## How to Use

First, define types that need type generation:

```csharp
public class FirstType
{
    public string StringProp { get; set; }
    public int IntProp { get; set; }
}

public class SecondType
{
    public string[] StringArray { get; set; }
    public FirstType FirstTypeProp { get; set; }
}
```

Then generate TypeScript files with:

```csharp
var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, new RootTypesProvider(typeof(SecondType)));
generatorGenerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "output"));
```

By default, this will generate file with name `.ts` with following content:

```ts
// tslint:disable
// TypeScriptContractGenerator's generated content

export type SecondType = {
    stringArray?: null | string[];
    firstTypeProp?: null | FirstType;
};
export type FirstType = {
    stringProp?: null | string;
    intProp: number;
};
```

If you want generated files to have different name or to generate some typings differently, you should pass your own implementation of `ICustomTypeGenerator` to `TypeScriptGenerator`.

## Generation options

### LinterDisableMode

Use `/* eslint-disable */` or `// tslint:disable` comment in generated files. `EsLint` is default option.

### EnableOptionalProperties

This option is **enabled** by default. When **enabled** produces optional properties for members which may contain nulls.

```ts
export type SomeType = {
    somePropertyWithNullableValue?: typeDefinition;
    somePropertyWithNonNullableValue: typeDefinition;
};

```
When **disabled**, all properties produced as required.

### UseGlobalNullable

This option is **disabled** by default. When **enabled**, global `Nullable<T>` is used instead of union `null | T`

### NullabilityMode

NullabilityMode has 4 options:
- None - all generated properties are not null
- Pessimistic (default) - generates `Nullable` property for properties that have no JetBrains nullability attributes
- Optimistic - generates not null property for properties that have no JetBrains nullability attributes
- NullableReference - generates not null properties based on C# 8 Nullable Reference Type attributes

Options `Pessimistic` or `Optimistic` can be combined with `NullableReference` option, this way generator first looks for C# 8 Nullable Reference Type attributes, then JetBrains nullability attributes

## Attributes

There is `ContractGeneratorIgnore` attribute that can be applied to properties and makes generator skip current property.

## Roslyn usage

To use Roslyn you should get a `Compilation` object of your project. It can be done with helper method `AdhocProject.GetCompilation(string[] directories, string[] assemblies)`.
You can then get customization info from this compilation by calling extension method `compilation.GetCustomization()` and call `TypeScriptGenerator` with this customization:
```csharp
var (customTypeGenerator, typesProvider) = AdhocProject.GetCompilation(directories, assemblies).GetCustomization();
var typeGenerator = new TypeScriptGenerator(options, customTypeGenerator, typesProvider);
typeGenerator.GenerateFiles(outputDirectory);
```

## dotnet tool usage

Install tool with command:

`dotnet tool install -g SkbKontur.TypeScript.ContractGenerator.Cli`

Use tool with command:

`dotnet ts-gen --assembly ./Api/bin/Api.dll --outputDir ./src/Api`

dotnet tool also supports Roslyn:

`dotnet ts-gen --directory ./Api;./Core --assembly ./External/Dependency.dll --outputDir ./src/Api`
