# TypeScript.ContractGenerator

[![NuGet Status](https://img.shields.io/nuget/v/SkbKontur.TypeScript.ContractGenerator.svg)](https://www.nuget.org/packages/SkbKontur.TypeScript.ContractGenerator/)
[![Build status](https://ci.appveyor.com/api/projects/status/1x5x9gw0a7h12g38/branch/master?svg=true)](https://ci.appveyor.com/project/skbkontur/typescript-contractgenerator/branch/master)

A tool that can generate TypeScript or Flow types from C# classes

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
generator.GenerateFiles("./output", JavaScriptTypeChecker.TypeScript);
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

### EnumGenerationMode

This options is set to `FixedStringsAndDictionary` by default.

```csharp
public enum EnumGenerationMode
{
    FixedStringsAndDictionary = 0,
    TypeScriptEnum = 1,
}
```

Setting option value equal to `FixedStringsAndDictionary` produces following output:

```ts
export type SomeEnum = 'A' | 'B';
export const SomeEnums = {
    ['A']: ('A') as SomeEnum,
    ['B']: ('B') as SomeEnum,
};
```

Option value `TypeScriptEnum` produces following:

```ts
export enum SomeEnum {
    A = 'A',
    B = 'B',
}
```

### EnableOptionalProperties

This option is **enabled** by default. When **enabled** produces optional properties for members which may contain nulls.

```ts
export type SomeType = {
    somePropertyWithNullableValue?: typeDefinition;
    somePropertyWithNonNullableValue: typeDefinition;
};

```
When **disabled**, all properties produced as required.

### EnableExplicitNullability

This option is **enabled** by default. When **enabled** produces nullable types for members which may contain nulls.

```ts
export type SomeType = {
    nullablePropertyDefinition: null | string;
    nonNullablePropertyDefinition: string;
};
```

When **disabled** produces all types as-is.

### UseGlobalNullable

This option is **disabled** by default. When **enabled**, global `Nullable<T>` is used instead of union `null | T`

### NullabilityMode

This option is set to `Pessimistic` by default. When set to `Pessimistic`, generates `Nullable` property for properties that have no nullability attributes. When set to `Optimistic`, generates not null property for properties that have no nullability attributes.

## Attributes

There are several attributes that can be applied to properties

* `ContractGeneratorIgnore` attribute makes generator skip current property.
* `ContractGeneratorInferValue` signals to generator that value in property is constant. This attribute can be used only in classes that have default parameterless constructor.
