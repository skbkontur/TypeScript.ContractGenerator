using System.Text;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptInterfaceDeclaration : TypeScriptTypeDeclaration
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