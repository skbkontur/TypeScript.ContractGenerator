using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class TypeScriptGeneratorHelpers
    {
        public static (bool, Type type) ProcessNullable(ICustomAttributeProvider attributeContainer, Type type, NullabilityMode nullabilityMode)
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

        class NullableOption
        {
            public bool[] levels { get; set; }

            public NullableOption()
            {
            }

            public bool isNullable()
            {
                return levels[0];
            }
            public bool innerTypeisNullable()
            {
                return levels.Length>1&&levels[1];
            }
            public bool innerTypeHaveInnerNullabletype()
            {
                return levels.Length>2&&levels[2];
            }
            
            public static implicit operator NullableOption(bool isNullable)
            {
                return new NullableOption { levels = new [] {isNullable}};
            }
            public static implicit operator bool(NullableOption nullableOption)
            {
                return nullableOption.levels[0];
            }
        }
        
        private static bool CanBeNull([NotNull] ICustomAttributeProvider attributeContainer, NullabilityMode nullabilityMode)
        {
            if (NullabilityMode.NullableReference == nullabilityMode)
            {
                var nullableAttribute = attributeContainer.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.Nullable);
                if (nullableAttribute is null)
                {
                    return false;
                }
                var flags = nullableAttribute.GetType().GetField("NullableFlags").GetValue(nullableAttribute) as byte[];
                
                if (flags != null )
                {
                    //return Enumerable.Range(0, 10).Select(index => flags.Length > 1 && flags[1] == 2).ToArray();
                    return flags[0] == 2;
                }
                
                // 1 -not null
                return false;
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
    }
}