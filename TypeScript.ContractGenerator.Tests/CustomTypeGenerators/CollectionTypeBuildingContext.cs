using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class CollectionTypeBuildingContext : ITypeBuildingContext
    {
        public CollectionTypeBuildingContext(ITypeInfo arrayType)
        {
            elementType = arrayType.GetGenericArguments()[0];
        }

        public static bool Accept(ITypeInfo typeInfo)
        {
            return typeInfo.IsGenericType &&
                   typeInfo.GetGenericArguments().Length == 1 &&
                   typeInfo.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(new TypeWrapper(typeof(ICollection<>))));
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator, null);
            return new TypeScriptArrayType(itemType);
        }

        private readonly ITypeInfo elementType;
    }
}