using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TypeScriptEnumTypeBuildingContext : TypeBuildingContext
    {
        public TypeScriptEnumTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var values = Type.GetEnumNames();
            var enumResult = new JavaScriptEnumDeclaration
                {
                    Name = Type.Name,
                    Definition = new JavaScriptEnumDefinition(
                        values.Select(x => new JavaScriptEnumMember {Name = x, ValueLiteral = new TypeScriptStringLiteral(x)})),
                };
            Unit.Body.Add(
                new TypeScriptExportTypeStatement
                    {
                        Declaration = enumResult
                    });
            Declaration = enumResult;
        }
    }
}