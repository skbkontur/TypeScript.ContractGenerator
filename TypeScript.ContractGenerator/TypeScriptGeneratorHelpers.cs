using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

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

        [NotNull]
        public static byte[] GetNullableFlags(ICustomAttributeProvider attributeContainer)
        {
            byte contextFlag = 0;
            if (attributeContainer is MemberInfo memberInfo)
                contextFlag = GetNullableContextFlag(memberInfo.ReflectedType);
            return GetNullableFlagsInternal(attributeContainer) ?? new[] {contextFlag};
        }

        private static byte[] GetNullableFlagsInternal(ICustomAttributeProvider attributeContainer)
        {
            var nullableAttribute = attributeContainer?.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.Nullable);
            return nullableAttribute?.GetType().GetField("NullableFlags").GetValue(nullableAttribute) as byte[];
        }

        private static byte GetNullableContextFlag(ICustomAttributeProvider attributeContainer)
        {
            var nullableAttribute = attributeContainer?.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.NullableContext);
            var flag = nullableAttribute?.GetType().GetField("Flag").GetValue(nullableAttribute);
            if (flag == null)
                return 0;
            return (byte)flag;
        }

        private static bool CanBeNull([NotNull] ICustomAttributeProvider attributeContainer, NullabilityMode nullabilityMode)
        {
            if (NullabilityMode.NullableReference == nullabilityMode)
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

        public static int GetGenericArgumentsToSkip(Type type)
        {
            if (type.IsArray)
                return 1 + GetGenericArgumentsToSkip(type.GetElementType());

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
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