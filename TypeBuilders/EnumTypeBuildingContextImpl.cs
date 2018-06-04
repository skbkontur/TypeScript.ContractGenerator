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
            var values = Type.GetEnumNames();
            var enumResult = new FlowTypeTypeDeclaration
                {
                    Name = Type.Name,
                    Definition = new FlowTypeUnionType(values.Select(x => new FlowTypeStringLiteralType(x)).Cast<FlowTypeType>().ToArray()),
                };
            Unit.Body.Add(
                new FlowTypeExportTypeStatement
                    {
                        Declaration = enumResult
                    });
            Unit.Body.Add(
                new FlowTypeExportStatement
                    {
                        Declaration = new FlowTypeConstantDefinition
                            {
                                Name = Type.Name + "s",
                                Value = new FlowTypeObjectLiteral(values.Select(x => new FlowTypeObjectLiteralProperty
                                    {
                                        Name = new FlowTypeStringLiteral {Value = x},
                                        Value = new FlowTypeCastExpression(new FlowTypeStringLiteral
                                            {
                                                Value = x,
                                            }, new FlowTypeTypeReference(Type.Name)),
                                    }))
                            }
                    }
                );
            Declaration = enumResult;
        }
    }
}