using System.Text;

namespace TypeScript.CodeDom
{
    public class FlowTypeInterfaceDeclaration : FlowTypeTypeDeclaration
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            var result = new StringBuilder();
            result.AppendFormat("interface {0} ", Name);
            result.Append(Definition.GenerateCode(context));
            return result.ToString();
        }
    }
}