using System;
using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : TypeBuildingContextBase
    {
        public ArrayTypeBuildingContext(ITypeInfo arrayType)
            : base(arrayType)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsArray || type.IsGenericType && enumerableTypes.Contains(type.GetGenericTypeDefinition());
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return new TypeScriptArrayType(typeGenerator.BuildAndImportType(targetUnit, GetElementType(type)));
        }

        private static ITypeInfo GetElementType(ITypeInfo arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType() ?? throw new ArgumentNullException($"Array type's {arrayType.Name} element type is not defined");

            if (arrayType.IsGenericType && enumerableTypes.Contains(arrayType.GetGenericTypeDefinition()))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }

        private static readonly ITypeInfo[] enumerableTypes = {TypeInfo.From(typeof(List<>)), TypeInfo.From(typeof(IEnumerable<>))};
    }
}