using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptGenericType : TypeScriptType
    {
        public TypeScriptGenericType(TypeScriptType typeReference, TypeScriptType[] genericParameters)
        {
            TypeReference = typeReference;
            GenericParameters = genericParameters;
        }

        public TypeScriptType[] GenericParameters { get; private set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return TypeReference.GenerateCode(context) + "<" + string.Join(", ", GenericParameters.Select(x => x.GenerateCode(context))) + ">";
        }

        public TypeScriptType TypeReference { get; private set; }
    }
}