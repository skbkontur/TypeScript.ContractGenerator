using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericTypeTypeBuildingContext : ITypeBuildingContext
    {
        public GenericTypeTypeBuildingContext(Type type, ICustomAttributeProvider customAttributeProvider, TypeScriptGenerationOptions options)
        {
            this.type = type;
            this.customAttributeProvider = customAttributeProvider;
            this.options = options;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }
//todo add nullable attribute handler
        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var typeReference = typeGenerator.ResolveType(type.GetGenericTypeDefinition()).ReferenceFrom(targetUnit, typeGenerator);
            var arguments = new List<TypeScriptType>();
            foreach (var argument in type.GetGenericArguments())
            {
                var targetType = typeGenerator.ResolveType(argument).ReferenceFrom(targetUnit, typeGenerator);
                if (options.UseGlobalNullable)
                {
                    var nullableAttribute = customAttributeProvider.GetCustomAttributes(true).SingleOrDefault(a => a.GetType().Name == AnnotationsNames.Nullable);
                    if (nullableAttribute is null)
                    {
                        //false;
                    }
                    var flags = nullableAttribute.GetType().GetField("NullableFlags").GetValue(nullableAttribute) as byte[];
                
                    if (flags != null )
                    {
                        //return Enumerable.Range(0, 10).Select(index => flags.Length > 1 && flags[1] == 2).ToArray();
                        
                    }
                
                    // 1 -not null
                
                }
                arguments.Add(targetType is INullabilityWrapperType nullabilityType ? nullabilityType.InnerType : targetType);
            }
            return new TypeScriptGenericTypeReference(typeReference as TypeScriptTypeReference, arguments.ToArray());
        }

        private readonly Type type;
        private readonly ICustomAttributeProvider customAttributeProvider;
        private readonly TypeScriptGenerationOptions options;
    }

    public class TypeScriptGenericTypeReference : TypeScriptType
    {
        public TypeScriptGenericTypeReference(TypeScriptTypeReference genericTypeReference, TypeScriptType[] genericArguments)
        {
            this.genericTypeReference = genericTypeReference;
            this.genericArguments = genericArguments;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{genericTypeReference.GenerateCode(context)}<{genericArguments.GenerateCodeCommaSeparated(context)}>";
        }

        private readonly TypeScriptTypeReference genericTypeReference;
        private readonly TypeScriptType[] genericArguments;
    }
}