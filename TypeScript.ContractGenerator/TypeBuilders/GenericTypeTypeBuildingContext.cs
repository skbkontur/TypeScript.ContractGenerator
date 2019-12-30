using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericTypeTypeBuildingContext : ITypeBuildingContext
    {
        public GenericTypeTypeBuildingContext(Type type,
                                              [CanBeNull] ICustomAttributeProvider customAttributeProvider,
                                              [NotNull] TypeScriptGenerationOptions options)
        {
            this.type = type;
            this.customAttributeProvider = customAttributeProvider;
            this.options = options;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var typeReference = typeGenerator.ResolveType(type.GetGenericTypeDefinition()).ReferenceFrom(targetUnit, typeGenerator);
            var arguments = new List<TypeScriptType>();
            var nullableIndex = 1;
            var nullableBytes = TypeScriptGeneratorHelpers.GetNullableFlags(customAttributeProvider) ?? new byte[0];
            foreach (var argument in type.GetGenericArguments())
            {
                var targetType = typeGenerator.ResolveType(argument).ReferenceFrom(targetUnit, typeGenerator);
                if (options.NullabilityMode == NullabilityMode.NullableReference)
                {
                    var isNullable = nullableBytes.Length == 1 && nullableBytes[0] == 2 || nullableBytes.Length > nullableIndex && nullableBytes[nullableIndex] == 2;
                    nullableIndex += TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(argument);
                    arguments.Add(TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(targetType, !argument.IsValueType && isNullable, options));
                }
                else
                {
                    arguments.Add(targetType is INullabilityWrapperType nullabilityType ? nullabilityType.InnerType : targetType);
                }
            }
            return new TypeScriptGenericTypeReference(typeReference as TypeScriptTypeReference, arguments.ToArray());
        }

        private readonly Type type;
        private readonly ICustomAttributeProvider customAttributeProvider;
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