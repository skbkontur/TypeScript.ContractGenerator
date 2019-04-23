using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstructorCallExpression : TypeScriptExpression
    {
        public TypeScriptConstructorCallExpression(TypeScriptExpression className, params TypeScriptExpression[] arguments)
        {
            ClassName = className;
            Arguments = new List<TypeScriptExpression>(arguments);
        }

        public TypeScriptExpression ClassName { get; }
        public List<TypeScriptExpression> Arguments { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"new {ClassName.GenerateCode(context)}({Arguments.GenerateCodeCommaSeparated(context)})";
        }
    }
}