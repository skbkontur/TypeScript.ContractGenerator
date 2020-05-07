using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public abstract class TypeBuildingContextBase : ITypeBuildingContext
    {
        protected TypeBuildingContextBase(ITypeInfo type)
        {
            Type = type;
        }

        protected ITypeInfo Type { get; private set; }

        public virtual bool IsDefinitionBuilt => true;

        public virtual void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public virtual void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            // if (!type.Equals(Type))
            // throw new InvalidOperationException($"Expected type {Type} with different meta, but got different type: {type}");

            return ReferenceFromInternal(type, targetUnit, typeGenerator);
        }

        protected abstract TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator);
    }

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
            return targetUnit.AddTypeImport(type, Declaration, Unit);
        }
    }
}