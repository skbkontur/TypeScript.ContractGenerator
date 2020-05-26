using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

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
            var attributeProvider = type.Member;
            var typeReference = typeGenerator.BuildAndImportType(targetUnit, Type.GetGenericTypeDefinition());
            var arguments = new List<TypeScriptType>();
            var nullableIndex = 1;
            foreach (var argument in Type.GetGenericArguments())
            {
                var targetType = typeGenerator.BuildAndImportType(targetUnit, argument);
                if (typeGenerator.Options.NullabilityMode == NullabilityMode.NullableReference)
                {
                    var isNullable = TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(attributeProvider, argument, nullableIndex);
                    nullableIndex += TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(argument);
                    arguments.Add(TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, !argument.IsValueType && isNullable, typeGenerator.Options));
                }
                else
                {
                    arguments.Add(targetType is INullabilityWrapperType nullabilityType ? nullabilityType.InnerType : targetType);
                }
            }
            return new TypeScriptGenericTypeReference((TypeScriptTypeReference)typeReference, arguments.ToArray());
        }
    }

    public class TypeScriptGenericTypeReference : TypeScriptType
    {
        public TypeScriptGenericTypeReference(TypeScriptTypeReference genericTypeReference, TypeScriptType[] genericArguments)
        {
            this.genericTypeReference = genericTypeReference;
            this.genericArguments = genericArguments;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{genericTypeReference.GenerateCode(context)}<{genericArguments.GenerateCodeCommaSeparated(context)}>";
        }

        private readonly TypeScriptTypeReference genericTypeReference;
        private readonly TypeScriptType[] genericArguments;
    }
}