using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericTypeTypeBuildingContext : ITypeBuildingContext
    {
        public GenericTypeTypeBuildingContext(ITypeInfo type, [NotNull] TypeScriptGenerationOptions options)
        {
            this.type = type;
            this.options = options;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider attributeProvider)
        {
            var typeReference = typeGenerator.ResolveType(type.GetGenericTypeDefinition()).ReferenceFrom(targetUnit, typeGenerator, null);
            var arguments = new List<TypeScriptType>();
            var nullableIndex = 1;
            foreach (var argument in type.GetGenericArguments())
            {
                var targetType = typeGenerator.ResolveType(argument).ReferenceFrom(targetUnit, typeGenerator, null);
                if (options.NullabilityMode == NullabilityMode.NullableReference)
                {
                    var isNullable = TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(attributeProvider, argument, nullableIndex);
                    nullableIndex += TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(argument);
                    arguments.Add(TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, !argument.Type.IsValueType && isNullable, options));
                }
                else
                {
                    arguments.Add(targetType is INullabilityWrapperType nullabilityType ? nullabilityType.InnerType : targetType);
                }
            }
            return new TypeScriptGenericTypeReference(typeReference as TypeScriptTypeReference, arguments.ToArray());
        }

        private readonly ITypeInfo type;
        private readonly TypeScriptGenerationOptions options;
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