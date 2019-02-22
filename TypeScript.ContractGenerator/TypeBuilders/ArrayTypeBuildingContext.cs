using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext(Type arrayType)
        {
            elementType = GetElementType(arrayType);
        }

        private Type GetElementType(Type arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType();

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition() == typeof(List<>))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }

        public static bool Accept(Type type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator);
            return new FlowTypeArrayType(itemType);
        }

        private readonly Type elementType;
    }
}