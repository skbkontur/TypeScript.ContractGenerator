using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DictionaryTypeBuildingContext : ITypeBuildingContext
    {
        public DictionaryTypeBuildingContext([NotNull] Type dictionaryType, [NotNull] TypeScriptGenerationOptions options)
        {
            keyType = dictionaryType.GetGenericArguments()[0];
            valueType = dictionaryType.GetGenericArguments()[1];
            this.options = options;
        }

        public static bool Accept(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
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
            return new TypeScriptTypeDefintion
                {
                    Members =
                        {
                            new TypeScriptTypePropertyGetterDeclaration
                                {
                                    Argument = new TypeScriptArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = GetKeyType(targetUnit, typeGenerator, customAttributeProvider),
                                        },
                                    ResultType = GetValueType(targetUnit, typeGenerator, customAttributeProvider),
                                    Optional = true,
                                }
                        }
                };
        }

        private TypeScriptType GetKeyType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider)
        {
            var key = typeGenerator.ResolveType(keyType).ReferenceFrom(targetUnit, typeGenerator, null);
            return MaybeNull(keyType, key, customAttributeProvider, 1);
        }

        private TypeScriptType GetValueType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider)
        {
            var value = typeGenerator.ResolveType(valueType).ReferenceFrom(targetUnit, typeGenerator, null);
            return MaybeNull(valueType, value, customAttributeProvider, 1 + TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(keyType));
        }

        private TypeScriptType MaybeNull(Type trueType, TypeScriptType type, ICustomAttributeProvider customAttributeProvider, int index)
        {
            if (options.NullabilityMode != NullabilityMode.NullableReference)
                return type;

            var isNullable = TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(customAttributeProvider, trueType, index);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(type, isNullable, options);
        }

        private readonly Type keyType;
        private readonly Type valueType;
        private readonly TypeScriptGenerationOptions options;
    }
}