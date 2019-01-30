using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TypeBuildingContext : ITypeBuildingContext
    {
        public TypeBuildingContext(FlowTypeUnit unit, Type type)
        {
            Unit = unit;
            Type = type;
        }

        public virtual void Initialize(ITypeGenerator typeGenerator)
        {
            Unit.Body.Add(new FlowTypeExportTypeStatement {Declaration = Declaration});
        }

        public FlowTypeTypeDeclaration Declaration { get; set; }
        public FlowTypeUnit Unit { get; set; }
        public Type Type { get; set; }
        public virtual bool IsDefinitionBuilded { get { return true; } }

        public virtual void BuildDefiniion(ITypeGenerator typeGenerator)
        {
        }

        public virtual FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return targetUnit.AddTypeImport(Type, Declaration, Unit);
        }
    }
}