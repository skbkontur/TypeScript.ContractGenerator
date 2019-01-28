using System.Collections.Generic;
using System.Text;

namespace TypeScript.CodeDom
{
    public class FlowTypeTypeDefintion : FlowTypeType
    {
        public List<FlowTypeTypeMemberDeclarationBase> Members { get { return members; } }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var result = new StringBuilder();
            result.AppendFormat("{{").Append(context.NewLine);
            foreach (var member in Members)
            {
                result.AppendWithTab(context.Tab, member.GenerateCode(context), context.NewLine).Append(context.NewLine);
            }
            result.Append("}");
            return result.ToString();
        }

        private readonly List<FlowTypeTypeMemberDeclarationBase> members = new List<FlowTypeTypeMemberDeclarationBase>();
    }
}