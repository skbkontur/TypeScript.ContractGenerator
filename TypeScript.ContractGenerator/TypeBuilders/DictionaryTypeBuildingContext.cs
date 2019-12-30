using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DictionaryTypeBuildingContext : ITypeBuildingContext
    {
        public DictionaryTypeBuildingContext([NotNull] Type dictionaryType,
                                             [CanBeNull] ICustomAttributeProvider customAttributeProvider,
                                             [NotNull] TypeScriptGenerationOptions options)
        {
            keyType = dictionaryType.GetGenericArguments()[0];
            valueType = dictionaryType.GetGenericArguments()[1];
            this.customAttributeProvider = customAttributeProvider;
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

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
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
                                            Type = GetKeyType(targetUnit, typeGenerator),
                                        },
                                    ResultType = GetValueType(targetUnit, typeGenerator),
                                    Optional = true,
                                }
                        }
                };
        }

        private TypeScriptType GetKeyType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var key = typeGenerator.ResolveType(keyType).ReferenceFrom(targetUnit, typeGenerator);
            return MaybeNull(key, 1);
        }

        private TypeScriptType GetValueType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var value = typeGenerator.ResolveType(valueType).ReferenceFrom(targetUnit, typeGenerator);
            return MaybeNull(value, 2);
        }

        private TypeScriptType MaybeNull(TypeScriptType type, int i)
        {
            if (options.NullabilityMode != NullabilityMode.NullableReference)
                return type;

            var nullableBytes = TypeScriptGeneratorHelpers.GetNullableFlags(customAttributeProvider) ?? new byte[0];
            if (nullableBytes.Length == 1 && nullableBytes[0] == 2 || nullableBytes.Length > i && nullableBytes[i] == 2)
                return new TypeScriptOrNullType(type);

            return type;
        }

        private readonly Type keyType;
        private readonly Type valueType;
        private readonly ICustomAttributeProvider customAttributeProvider;
        private readonly TypeScriptGenerationOptions options;
    }
}