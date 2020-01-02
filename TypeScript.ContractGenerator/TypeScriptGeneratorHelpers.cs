using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class TypeScriptGeneratorHelpers
    {
        public static (bool, Type) ProcessNullable(ICustomAttributeProvider attributeContainer, Type type, NullabilityMode nullabilityMode)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                return (true, underlyingType);
            }

            if (attributeContainer == null || !type.IsClass && !type.IsInterface)
                return (false, type);

            return (CanBeNull(attributeContainer, nullabilityMode), type);
        }

        public static bool NullableReferenceCanBeNull(ICustomAttributeProvider attributeContainer, ITypeInfo type, int index)
        {
            var nullableBytes = GetNullableFlags(attributeContainer);
            if (nullableBytes.Length == 1 && nullableBytes[0] == 2 || nullableBytes.Length > index && nullableBytes[index] == 2)
                return !type.IsValueType;
            return false;
        }

        [NotNull]
        public static byte[] GetNullableFlags(ICustomAttributeProvider attributeContainer)
        {
            byte contextFlag = 0;
            if (attributeContainer is MemberInfo memberInfo)
                contextFlag = GetNullableContextFlag(memberInfo) ?? GetNullableContextFlag(memberInfo.ReflectedType) ?? 0;
            return GetNullableFlagsInternal(attributeContainer) ?? new[] {contextFlag};
        }

        private static byte[] GetNullableFlagsInternal(ICustomAttributeProvider attributeContainer)
        {
            var nullableAttribute = attributeContainer?.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.Nullable);
            return nullableAttribute?.GetType().GetField("NullableFlags").GetValue(nullableAttribute) as byte[];
        }

        private static byte? GetNullableContextFlag(ICustomAttributeProvider attributeContainer)
        {
            var nullableAttribute = attributeContainer?.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.NullableContext);
            var flag = nullableAttribute?.GetType().GetField("Flag").GetValue(nullableAttribute);
            return (byte?)flag;
        }

        private static bool CanBeNull([NotNull] ICustomAttributeProvider attributeContainer, NullabilityMode nullabilityMode)
        {
            if (nullabilityMode == NullabilityMode.NullableReference)
            {
                var flags = GetNullableFlags(attributeContainer);
                return flags[0] == 2;
            }

            return
                nullabilityMode == NullabilityMode.Pessimistic
                    ? !attributeContainer.IsNameDefined(AnnotationsNames.NotNull) && !attributeContainer.IsNameDefined(AnnotationsNames.Required)
                    : attributeContainer.IsNameDefined(AnnotationsNames.CanBeNull);
        }

        [NotNull]
        public static TypeScriptType BuildTargetNullableTypeByOptions([NotNull] TypeScriptType innerType, bool isNullable, [NotNull] TypeScriptGenerationOptions options)
        {
            if (!(innerType is INullabilityWrapperType) && isNullable && options.EnableExplicitNullability)
            {
                if (!options.UseGlobalNullable)
                    return new TypeScriptOrNullType(innerType);

                if (options.UseGlobalNullable)
                    return new TypeScriptNullableType(innerType);
            }

            return innerType;
        }

        public static int GetGenericArgumentsToSkip(ITypeInfo type)
        {
            if (type.IsArray)
                return 1 + GetGenericArgumentsToSkip(type.GetElementType());

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(new TypeWrapper(typeof(Nullable<>))))
                return 0;

            if (!type.IsGenericType)
                return type.IsValueType ? 0 : 1;

            var count = 1;
            foreach (var argument in type.GetGenericArguments())
                count += GetGenericArgumentsToSkip(argument);

            return count;
        }
    }
}