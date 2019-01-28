using System;

using SKBKontur.Catalogue.FlowType.CodeDom;
using SKBKontur.Catalogue.FlowType.ContractGenerator;
using SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.CustomTypeGenerators
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