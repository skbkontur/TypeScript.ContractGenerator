using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TypeBuildingContext : TypeBuildingContextBase
    {
        protected TypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(type)
        {
            Unit = unit;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Unit.Body.Add(new TypeScriptExportTypeStatement {Declaration = Declaration});
        }

        protected TypeScriptTypeDeclaration Declaration { get; set; }

        protected TypeScriptUnit Unit { get; }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return targetUnit.AddTypeImport(type, Declaration, Unit, typeGenerator.Options.UseTypeImports);
        }
    }
}
