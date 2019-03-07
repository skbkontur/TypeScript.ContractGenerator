using System;
using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class CollectionTypeBuildingContext : ITypeBuildingContext
    {
        public CollectionTypeBuildingContext(Type arrayType)
        {
            elementType = arrayType.GetGenericArguments()[0];
        }

        public static bool Accept(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericArguments().Length == 1 &&
                   type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator);
            return new TypeScriptArrayType(itemType);
        }

        private readonly Type elementType;
    }
}