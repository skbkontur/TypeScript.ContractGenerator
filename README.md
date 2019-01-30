# TypeScript.ContractGenerator

[![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator/)
[![Build status](https://ci.appveyor.com/api/projects/status/1x5x9gw0a7h12g38?svg=true)](https://ci.appveyor.com/project/skbkontur/typescript-contractgenerator)

A tool that can generate TypeScript or Flow types from C# classes

## Release Notes

See [CHANGELOG](CHANGELOG.md).

## How to Use

First, define types that need type generation:

```
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

```
var generator = new FlowTypeGenerator(customTypeGenerator : null, rootTypes : new[] {typeof(SecondType)});
generator.GenerateTypeScriptFiles("./output");
```

By default, this will generate file with name `.tsx` with following content:

```
// TypeScriptContractGenerator's generated content
// tslint:disable

export type SecondType = {
    stringArray?: null | string[];
    firstTypeProp?: null | FirstType;
};
export type FirstType = {
    stringProp?: null | string;
    intProp: number;
};
```

If you want generated files to have different name or to generate some typings differently, you should pass your own implementation of `ICustomTypeGenerator` to `FlowTypeGenerator`.
