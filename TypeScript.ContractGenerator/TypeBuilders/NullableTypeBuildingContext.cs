using System;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class NullableTypeBuildingContext : TypeBuildingContextBase
    {
        public NullableTypeBuildingContext(ITypeInfo type)
            : base(type)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Nullable<>)));
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemTypeScriptType = typeGenerator.BuildAndImportType(targetUnit, type.GetGenericArguments()[0]);
            if (typeGenerator.Options.NullabilityMode == NullabilityMode.None)
                return itemTypeScriptType;

            return typeGenerator.Options.UseGlobalNullable
                       ? (TypeScriptType)new TypeScriptNullableType(itemTypeScriptType)
                       : new TypeScriptOrNullType(itemTypeScriptType);
        }
    }
}