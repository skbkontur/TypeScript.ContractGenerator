# TypeScript.ContractGenerator

A tool that can generate TypeScript types from C# classes

|                                     | Build Status |
|-------------------------------------|:--------------: |
| TypeScript.ContractGenerator        | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator/) |
| TypeScript.ContractGenerator.Roslyn | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.Roslyn.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator.Roslyn/) |
| TypeScript.ContractGenerator.Cli    | [![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.Cli.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator.Cli/) |
| Build                               | [![Build status](https://github.com/skbkontur/TypeScript.ContractGenerator/actions/workflows/actions.yml/badge.svg)](https://github.com/skbkontur/TypeScript.ContractGenerator/actions) |


## Release Notes

See [CHANGELOG](CHANGELOG.md).

## How to Use

See https://github.com/skbkontur/TypeScript.ContractGenerator/wiki#how-to-use

See [Quick Start](https://github.com/skbkontur/TypeScript.ContractGenerator/wiki/Quick-Start) for more detailed guide

## Generation options

See https://github.com/skbkontur/TypeScript.ContractGenerator/wiki/Generation-Options

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
