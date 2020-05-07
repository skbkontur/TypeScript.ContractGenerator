using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class NullableTypeBuildingContext : TypeBuildingContextBase
    {
        public NullableTypeBuildingContext(ITypeInfo nullableUnderlyingType, TypeScriptGenerationOptions options)
            : base(nullableUnderlyingType)
        {
            this.options = options;
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemTypeScriptType = typeGenerator.ReferenceFrom(Type, targetUnit);
            return options.UseGlobalNullable
                       ? (TypeScriptType)new TypeScriptNullableType(itemTypeScriptType)
                       : new TypeScriptOrNullType(itemTypeScriptType);
        }

        private readonly TypeScriptGenerationOptions options;
    }
}