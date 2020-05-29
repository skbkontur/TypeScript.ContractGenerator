using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericParameterTypeBuildingContext : TypeBuildingContextBase
    {
        public GenericParameterTypeBuildingContext(ITypeInfo type)
            : base(type)
        {
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return new TypeScriptTypeReference(Type.Name);
        }
    }
}