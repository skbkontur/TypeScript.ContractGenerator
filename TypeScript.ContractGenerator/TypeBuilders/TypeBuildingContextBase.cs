using System;

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

        protected ITypeInfo Type { get; }

        public virtual bool IsDefinitionBuilt => true;

        public virtual void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public virtual void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            if (!type.Equals(Type))
                throw new InvalidOperationException($"Expected type {Type} with different meta, but got different type: {type}");

            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(
                ReferenceFromInternal(type, targetUnit, typeGenerator),
                type.CanBeNull(typeGenerator.Options.NullabilityMode),
                typeGenerator.Options);
        }

        protected abstract TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator);
    }
}