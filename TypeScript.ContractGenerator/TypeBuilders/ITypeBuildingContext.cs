using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public interface ITypeBuildingContext
    {
        bool IsDefinitionBuilt { get; }
        void Initialize(ITypeGenerator typeGenerator);
        void BuildDefinition(ITypeGenerator typeGenerator);
        TypeScriptType ReferenceFrom(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator);
    }
}