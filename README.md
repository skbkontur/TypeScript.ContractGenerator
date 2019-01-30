# TypeScript.ContractGenerator

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
