# Migration

## Options changes

- `Flow` types generation is removed
- `EnableExplicitNullability` option is replaced with `NullabilityMode.None` option
- `EnumGenerationMode` and `Pluralize` options are removed
- `NullabilityMode` has flags, which means that combinations such as `Pessimistic | NullableReference` can be used

## New abstractions

All reflection types used in TypeScript generation (e.g. `Type`, `PropertyInfo`), were replaced by new abstractions.
The main abstraction is `ITypeInfo` which besides `Type` information also has nullability information and info about property/method that has this specific type.

`ITypeInfo` has Reflection and Roslyn implementation.
 It also has equality methods and can be constructed from Reflection type by calling `TypeInfo.From<T>()`.

To migrate custom generation code to new version, you should first replace all occurrences of these types: 
- `Type` → `ITypeInfo`
- `PropertyInfo` → `IPropertyInfo`
- `MethodInfo` → `IMethodInfo`
- `ParameterInfo` → `IParameterInfo`
- `FieldInfo` → `IFieldInfo`
- `ICustomAttributeProvider` → `IAttributeProvider`
- `typeof(T)` → `TypeInfo.From<T>()` or `TypeInfo.From(typeof(T))`
- `type == typeof(T)` → `type.Equals(TypeInfo.From<T>())`

## Api changes

- Generated type nullability is determined in `ReferenceFrom` method. In most cases it is better to inherit from `TypeBuildingContextBase` and implement `ReferenceFromInternal` method to avoid dealing with nullability issues. 

- Parameter `customAttributeProvider` was removed from `ReferenceFrom` method. `IAttributeProvider` can be accessed using `type.Member` property. 

```diff
public interface ITypeBuildingContext
{
-   TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider);
+   TypeScriptType ReferenceFrom(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator);
}
```

- `BuildAndImportType` is now an extension method for `ITypeGenerator`, `customAttributeProvider` parameter was also removed.

```diff
public interface ITypeGenerator
{
-   TypeScriptType BuildAndImportType(TypeScriptUnit targetUnit, ICustomAttributeProvider? customAttributeProvider, Type type);
}

public static class TypeScriptGeneratorHelpers
{
+   public static TypeScriptType BuildAndImportType(this ITypeGenerator typeGenerator, TypeScriptUnit typeScriptUnit, ITypeInfo type)
+   {
+       return typeGenerator.ResolveType(type).ReferenceFrom(type, typeScriptUnit, typeGenerator);
+   }
}
```

## Roslyn

When using Roslyn to generate TypeScript, Customization code is preprocessed, replacing `TypeInfo.From<T>()` and `TypeInfo.From(typeof(T))` invocations with `RoslynTypeInfo` instances and then compiled to in-memory assembly. Therefore, several changes to customization code are required to use Roslyn:

- There should be no `typeof(T)` invocations not wrapped with `TypeInfo.From()`
- All customization-related code should be in the same namespace
- Customization-related code should not depend on any third party packages
