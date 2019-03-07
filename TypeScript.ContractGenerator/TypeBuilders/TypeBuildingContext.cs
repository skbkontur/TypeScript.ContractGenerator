using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TypeBuildingContext : ITypeBuildingContext
    {
        protected TypeBuildingContext(TypeScriptUnit unit, Type type)
        {
            Unit = unit;
            Type = type;
        }

        public virtual void Initialize(ITypeGenerator typeGenerator)
        {
            Unit.Body.Add(new TypeScriptExportTypeStatement {Declaration = Declaration});
        }

        protected TypeScriptTypeDeclaration Declaration { get; set; }

        protected TypeScriptUnit Unit { get; }

        protected Type Type { get; }

        public virtual bool IsDefinitionBuilt => true;

        public virtual void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public virtual TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return targetUnit.AddTypeImport(Type, Declaration, Unit);
        }
    }
}