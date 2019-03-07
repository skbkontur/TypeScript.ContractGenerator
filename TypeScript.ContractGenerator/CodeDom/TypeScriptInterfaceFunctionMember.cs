using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptInterfaceFunctionMember
    {
        public string Name { get; set; }
        public List<TypeScriptArgumentDeclaration> Arguments => arguments;
        public TypeScriptType Result { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("{0}({1}): {2}", Name, GenerateArgumentListCode(context), Result.GenerateCode(context));
        }

        private string GenerateArgumentListCode(ICodeGenerationContext context)
        {
            return string.Join(", ", Arguments.Select(x => x.GenerateCode(context)));
        }

        private readonly List<TypeScriptArgumentDeclaration> arguments = new List<TypeScriptArgumentDeclaration>();
    }
}