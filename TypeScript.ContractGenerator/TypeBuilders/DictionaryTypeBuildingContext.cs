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
            var genericArgs = type.GetGenericArguments();
            var keyType = typeGenerator.BuildAndImportType(targetUnit, genericArgs[0]);
            var valueType = typeGenerator.BuildAndImportType(targetUnit, genericArgs[1]);
            if (typeGenerator.Options.NullabilityMode != NullabilityMode.NullableReference)
            {
                keyType = keyType.NotNull();
                valueType = valueType.NotNull();
            }
            return new TypeScriptTypeDefintion
                {
                    Members =
                        {
                            new TypeScriptTypePropertyGetterDeclaration
                                {
                                    Argument = new TypeScriptArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = keyType,
                                        },
                                    ResultType = valueType,
                                    Optional = true,
                                }
                        }
                };
        }
    }
}