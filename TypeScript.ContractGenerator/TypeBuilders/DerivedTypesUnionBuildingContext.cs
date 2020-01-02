using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DerivedTypesUnionBuildingContext : TypeBuildingContext
    {
        public DerivedTypesUnionBuildingContext(TypeScriptUnit unit, ITypeInfo type, bool useAbstractChildren = false)
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
            var type = Type.Type;
            var types = type.Assembly
                            .GetTypes()
                            .Where(x => x != type && type.IsAssignableFrom(x) && (useAbstractChildren || !x.IsAbstract))
                            .Select(x => typeGenerator.ResolveType(new TypeWrapper(x)).ReferenceFrom(Unit, typeGenerator, null))
                            .ToArray();
            Declaration.Definition = new TypeScriptUnionType(types);
        }

        private readonly bool useAbstractChildren;
    }
}