using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public interface ITypeBuildingContext
    {
        bool IsDefinitionBuilt { get; }
        void Initialize(ITypeGenerator typeGenerator);
        void BuildDefinition(ITypeGenerator typeGenerator);
        TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator);
    }
}