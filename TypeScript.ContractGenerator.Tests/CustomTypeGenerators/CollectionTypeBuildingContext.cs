using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class CollectionTypeBuildingContext : TypeBuildingContextBase
    {
        public CollectionTypeBuildingContext(ITypeInfo arrayType)
            : base(arrayType)
        {
        }

        public static bool Accept(ITypeInfo typeInfo)
        {
            return typeInfo.IsGenericType &&
                   typeInfo.GetGenericArguments().Length == 1 &&
                   typeInfo.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(ICollection<>))));
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var elementType = type.GetGenericArguments()[0];
            var itemType = typeGenerator.BuildAndImportType(targetUnit, elementType);
            return new TypeScriptArrayType(itemType);
        }
    }
}