using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class RedirectToTypeBuildingContext : TypeBuildingContextBase
    {
        public RedirectToTypeBuildingContext(string typeName, string path, ITypeInfo type)
            : base(type)
        {
            this.typeName = typeName;
            this.path = path;
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return targetUnit.AddTypeImport(type, new TypeScriptInterfaceDeclaration {Name = typeName}, new TypeScriptUnit {Path = path}, typeGenerator.Options.UseTypeImports);
        }

        private readonly string typeName;
        private readonly string path;
    }
}
