using System;
using System.Linq;

using SKBKontur.Catalogue.FlowType.CodeDom;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders
{
    internal class EnumTypeBuildingContextImpl : TypeBuildingContext
    {
        public EnumTypeBuildingContextImpl(FlowTypeUnit unit, Type type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var enumType = Type;
            Unit.Body.Add(new FlowTypeExportStatement
                {
                    Declaration = new FlowTypeConstantDefinition
                        {
                            Name = enumType.Name + "s",
                            Value = new FlowTypeObjectLiteral(Type.GetEnumNames().Select(x => new FlowTypeObjectLiteralProperty
                                {
                                    Name = new FlowTypeStringLiteral {Value = x},
                                    Value = new FlowTypeStringLiteral {Value = x},
                                }))
                        }
                });
            Declaration = new FlowTypeTypeDeclaration
                {
                    Name = enumType.Name,
                    Definition = new FlowTypeTypeKeysOfType
                        {
                            TargetType = new FlowTypeTypeOfValue
                                {
                                    TargetValue = new FlowTypeVariableReference(Type.Name + "s"),
                                }
                        },
                };
            Unit.Body.Add(new FlowTypeExportTypeStatement {Declaration = Declaration});
        }
    }
}