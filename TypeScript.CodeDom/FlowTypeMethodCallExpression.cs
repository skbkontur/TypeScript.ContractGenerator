using System.Collections.Generic;
using System.Linq;

namespace TypeScript.CodeDom
{
    public class FlowTypeMethodCallExpression : FlowTypeExpression
    {
        public FlowTypeMethodCallExpression(FlowTypeExpression subject, string methodName, params FlowTypeExpression[] arguments)
        {
            Subject = subject;
            MethodName = methodName;
            Arguments.AddRange(arguments);
        }

        public FlowTypeExpression Subject { get; set; }
        public string MethodName { get; set; }
        public List<FlowTypeExpression> Arguments { get { return arguments; } }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("{0}.{1}({2})", Subject.GenerateCode(context), MethodName, string.Join(", ", Arguments.Select(x => x.GenerateCode(context))));
        }

        private readonly List<FlowTypeExpression> arguments = new List<FlowTypeExpression>();
    }
}