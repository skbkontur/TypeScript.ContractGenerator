using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class DictionaryTypeBuildingContext : ITypeBuildingContext
    {
        public DictionaryTypeBuildingContext(Type dictionaryType)
        {
            keyType = dictionaryType.GetGenericArguments()[0];
            valueType = dictionaryType.GetGenericArguments()[1];
        }

        public bool IsDefinitionBuilded => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefiniion(ITypeGenerator typeGenerator)
        {
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var keyFlowType = typeGenerator.ResolveType(keyType).ReferenceFrom(targetUnit, typeGenerator);
            var valueFlowType = typeGenerator.ResolveType(valueType).ReferenceFrom(targetUnit, typeGenerator);

            return new FlowTypeTypeDefintion
                {
                    Members =
                        {
                            new FlowTypeTypePropertyGetterDeclaration
                                {
                                    Argument = new FlowTypeArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = keyFlowType,
                                        },
                                    ResultType = valueFlowType,
                                    Optional = true,
                                }
                        }
                };
        }

        private readonly Type keyType;
        private readonly Type valueType;
    }
}