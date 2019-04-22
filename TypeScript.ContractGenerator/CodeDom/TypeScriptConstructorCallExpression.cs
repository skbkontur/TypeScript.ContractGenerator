using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstructorCallExpression : TypeScriptExpression
    {
        public TypeScriptConstructorCallExpression(TypeScriptExpression className, params TypeScriptExpression[] arguments)
        {
            ClassName = className;
            Arguments.AddRange(arguments);
        }

        public TypeScriptExpression ClassName { get; set; }
        public List<TypeScriptExpression> Arguments => arguments;

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("new {0}({1})", ClassName.GenerateCode(context), string.Join(", ", Arguments.Select(x => x.GenerateCode(context))));
        }

        private readonly List<TypeScriptExpression> arguments = new List<TypeScriptExpression>();
    }
}