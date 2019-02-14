using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TypeBuildingContext : ITypeBuildingContext
    {
        protected TypeBuildingContext(FlowTypeUnit unit, Type type)
        {
            Unit = unit;
            Type = type;
        }

        public virtual void Initialize(ITypeGenerator typeGenerator)
        {
            Unit.Body.Add(new FlowTypeExportTypeStatement {Declaration = Declaration});
        }

        protected FlowTypeTypeDeclaration Declaration { get; set; }
        protected FlowTypeUnit Unit { get; }
        protected Type Type { get; }

        public virtual bool IsDefinitionBuilt => true;

        public virtual void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public virtual FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return targetUnit.AddTypeImport(Type, Declaration, Unit);
        }
    }
}