using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericTypeTypeBuildingContext : TypeBuildingContextBase
    {
        public GenericTypeTypeBuildingContext(ITypeInfo type)
            : base(type)
        {
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var typeReference = typeGenerator.BuildAndImportType(targetUnit, type.GetGenericTypeDefinition());
            return new TypeScriptGenericTypeReference(
                (TypeScriptTypeReference)typeReference,
                type.GetGenericArguments().Select(x => GetArgumentType(x, typeGenerator, targetUnit)).ToArray()
            );
        }

        private static TypeScriptType GetArgumentType(ITypeInfo argument, ITypeGenerator typeGenerator, TypeScriptUnit targetUnit)
        {
            var targetType = typeGenerator.BuildAndImportType(targetUnit, argument);
            if (typeGenerator.Options.NullabilityMode == NullabilityMode.NullableReference)
                return targetType;
            return targetType.NotNull();
        }
    }
}