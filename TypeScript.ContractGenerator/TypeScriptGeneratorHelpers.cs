using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class TypeScriptGeneratorHelpers
    {
        public static TypeScriptType BuildAndImportType(this ITypeGenerator typeGenerator, TypeScriptUnit typeScriptUnit, ITypeInfo type)
        {
            return typeGenerator.ResolveType(type).ReferenceFrom(type, typeScriptUnit, typeGenerator);
        }

        public static TypeScriptType NotNull(this TypeScriptType type)
        {
            return type is INullabilityWrapperType nullableType ? nullableType.InnerType : type;
        }

        public static TypeScriptType BuildTargetNullableTypeByOptions(TypeScriptType innerType, bool isNullable, TypeScriptGenerationOptions options)
        {
            if (!(innerType is INullabilityWrapperType) && isNullable && options.NullabilityMode != NullabilityMode.None)
            {
                if (!options.UseGlobalNullable)
                    return new TypeScriptOrNullType(innerType);

                if (options.UseGlobalNullable)
                    return new TypeScriptNullableType(innerType);
            }

            return innerType;
        }
    }
}