using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class FixedStringsAndDictionaryTypeBuildingContext : TypeBuildingContext
    {
        public FixedStringsAndDictionaryTypeBuildingContext(TypeScriptUnit unit, Type type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var values = Type.GetEnumNames();
            var enumResult = new TypeScriptTypeDeclaration
                {
                    Name = Type.Name,
                    Definition = new TypeScriptUnionType(values.Select(x => new TypeScriptStringLiteralType(x)).Cast<TypeScriptType>().ToArray()),
                };
            Unit.Body.Add(
                new TypeScriptExportTypeStatement
                    {
                        Declaration = enumResult
                    });
            Unit.Body.Add(
                new TypeScriptExportStatement
                    {
                        Declaration = new TypeScriptConstantDefinition
                            {
                                Name = Type.Name + "s",
                                Value = new TypeScriptObjectLiteral(values.Select(x => new TypeScriptObjectLiteralProperty
                                    {
                                        Name = new TypeScriptStringLiteral {Value = x},
                                        Value = new TypeScriptCastExpression(new TypeScriptStringLiteral
                                            {
                                                Value = x,
                                            }, new TypeScriptTypeReference(Type.Name)),
                                    }))
                            }
                    }
                );
            Declaration = enumResult;
        }
    }
}