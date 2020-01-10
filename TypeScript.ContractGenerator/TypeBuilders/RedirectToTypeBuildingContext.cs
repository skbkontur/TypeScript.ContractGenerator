using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class RedirectToTypeBuildingContext : ITypeBuildingContext
    {
        public RedirectToTypeBuildingContext(string typeName, string path, ITypeInfo type)
        {
            this.typeName = typeName;
            this.path = path;
            this.type = type;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider attributeProvider)
        {
            return targetUnit.AddTypeImport(type, new TypeScriptInterfaceDeclaration {Name = typeName}, new TypeScriptUnit {Path = path});
        }

        private readonly string typeName;
        private readonly string path;
        private readonly ITypeInfo type;
    }
}