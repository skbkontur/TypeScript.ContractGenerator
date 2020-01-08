using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DerivedTypesUnionBuildingContext : TypeBuildingContext
    {
        public DerivedTypesUnionBuildingContext(TypeScriptUnit unit, Type type, bool useAbstractChildren = false)
            : base(unit, type)
        {
            this.useAbstractChildren = useAbstractChildren;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = new TypeScriptTypeDeclaration
                {
                    Name = Type.Name,
                    Definition = null
                };
            Unit.Body.Add(new TypeScriptExportStatement {Declaration = Declaration});
        }

        public override bool IsDefinitionBuilt => Declaration.Definition != null;

        public override void BuildDefinition(ITypeGenerator typeGenerator)
        {
            var types = Type.Assembly
                            .GetTypes()
                            .Where(x => x != Type && Type.IsAssignableFrom(x) && (useAbstractChildren || !x.IsAbstract))
                            .Select(x => typeGenerator.ResolveType(x).ReferenceFrom(Unit, typeGenerator, null))
                            .ToArray();
            Declaration.Definition = new TypeScriptUnionType(types);
        }

        private readonly bool useAbstractChildren;
    }
}