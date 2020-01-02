using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext([NotNull] ITypeInfo arrayType, [NotNull] TypeScriptGenerationOptions options)
        {
            elementType = GetElementType(arrayType);
            this.options = options;
        }

        [NotNull]
        private ITypeInfo GetElementType([NotNull] ITypeInfo arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType() ?? throw new ArgumentNullException($"Array type's {arrayType.Name} element type is not defined");

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition().Type == typeof(List<>))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition().Type == typeof(List<>);
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator, null);
            var resultType = TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(itemType, CanItemBeNull(customAttributeProvider), options);
            return new TypeScriptArrayType(resultType);
        }

        private bool CanItemBeNull(ICustomAttributeProvider customAttributeProvider)
        {
            if (elementType.IsValueType || elementType.IsEnum || customAttributeProvider == null)
                return false;

            if (options.NullabilityMode == NullabilityMode.NullableReference)
                return TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(customAttributeProvider, elementType.Type, 1);

            return options.NullabilityMode == NullabilityMode.Pessimistic
                       ? !customAttributeProvider.IsNameDefined(AnnotationsNames.ItemNotNull)
                       : customAttributeProvider.IsNameDefined(AnnotationsNames.ItemCanBeNull);
        }

        [NotNull]
        private readonly TypeScriptGenerationOptions options;

        [NotNull]
        private readonly ITypeInfo elementType;
    }
}