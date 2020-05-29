using System;
using System.Collections.Generic;

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
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(List<>)));
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            return new TypeScriptArrayType(typeGenerator.BuildAndImportType(targetUnit, GetElementType(type)));
        }

        private static ITypeInfo GetElementType(ITypeInfo arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType() ?? throw new ArgumentNullException($"Array type's {arrayType.Name} element type is not defined");

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(List<>))))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }
    }
}