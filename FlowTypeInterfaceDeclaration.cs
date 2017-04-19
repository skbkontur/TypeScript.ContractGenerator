using System.Text;

namespace SKBKontur.Catalogue.FlowType.CodeDom
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