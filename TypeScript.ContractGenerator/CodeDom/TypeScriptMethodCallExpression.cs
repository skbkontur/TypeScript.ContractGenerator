using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptMethodCallExpression : TypeScriptExpression
    {
        public TypeScriptMethodCallExpression(TypeScriptExpression subject, string methodName, params TypeScriptExpression[] arguments)
        {
            Subject = subject;
            MethodName = methodName;
            Arguments.AddRange(arguments);
        }

        public TypeScriptExpression Subject { get; set; }
        public string MethodName { get; set; }
        public List<TypeScriptExpression> Arguments => arguments;

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("{0}.{1}({2})", Subject.GenerateCode(context), MethodName, string.Join(", ", Arguments.Select(x => x.GenerateCode(context))));
        }

        private readonly List<TypeScriptExpression> arguments = new List<TypeScriptExpression>();
    }
}