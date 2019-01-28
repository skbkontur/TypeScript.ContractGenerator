using System;
using System.Collections.Generic;
using System.Linq;
using TypeScript.CodeDom;

namespace TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericTypeTypeBuildingContextImpl : ITypeBuildingContext
    {
        public GenericTypeTypeBuildingContextImpl(Type type)
        {
            this.type = type;
        }

        public bool IsDefinitionBuilded => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefiniion(ITypeGenerator typeGenerator)
        {
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var typeReference = typeGenerator.ResolveType(type.GetGenericTypeDefinition()).ReferenceFrom(targetUnit, typeGenerator);
            var arguments = new List<FlowTypeType>();
            foreach (var argument in type.GetGenericArguments())
            {
                arguments.Add(typeGenerator.ResolveType(argument).ReferenceFrom(targetUnit, typeGenerator));
            }
            return new FlowTypeGenericTypeReference(typeReference as FlowTypeTypeReference, arguments.ToArray());
        }

        private readonly Type type;
    }

    public class FlowTypeGenericTypeReference : FlowTypeType
    {
        public FlowTypeGenericTypeReference(FlowTypeTypeReference genericTypeReference, FlowTypeType[] genericArguments)
        {
            this.genericTypeReference = genericTypeReference;
            this.genericArguments = genericArguments;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{genericTypeReference.GenerateCode(context)}<{string.Join(", ", genericArguments.Select(x => x.GenerateCode(context)))}>";
        }

        private readonly FlowTypeTypeReference genericTypeReference;
        private readonly FlowTypeType[] genericArguments;
    }
}