using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DictionaryTypeBuildingContext : TypeBuildingContextBase
    {
        public DictionaryTypeBuildingContext(ITypeInfo dictionaryType)
            : base(dictionaryType)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Dictionary<,>)));
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var attributeProvider = type.Member;
            var keyType = Type.GetGenericArguments()[0];
            var valueType = Type.GetGenericArguments()[1];
            return new TypeScriptTypeDefintion
                {
                    Members =
                        {
                            new TypeScriptTypePropertyGetterDeclaration
                                {
                                    Argument = new TypeScriptArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = GetKeyType(keyType, targetUnit, typeGenerator, attributeProvider),
                                        },
                                    ResultType = GetValueType(keyType, valueType, targetUnit, typeGenerator, attributeProvider),
                                    Optional = true,
                                }
                        }
                };
        }

        private TypeScriptType GetKeyType(ITypeInfo keyType, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider? attributeProvider)
        {
            var key = typeGenerator.BuildAndImportType(targetUnit, keyType);
            return MaybeNull(keyType, key, typeGenerator.Options, attributeProvider, 1);
        }

        private TypeScriptType GetValueType(ITypeInfo keyType, ITypeInfo valueType, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider? attributeProvider)
        {
            var value = typeGenerator.BuildAndImportType(targetUnit, valueType);
            return MaybeNull(valueType, value, typeGenerator.Options, attributeProvider, 1 + TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(keyType));
        }

        private TypeScriptType MaybeNull(ITypeInfo trueType, TypeScriptType type, TypeScriptGenerationOptions options, IAttributeProvider? attributeProvider, int index)
        {
            if (options.NullabilityMode != NullabilityMode.NullableReference)
                return type;

            var isNullable = TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(attributeProvider, trueType, index);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(type, isNullable, options);
        }
    }
}