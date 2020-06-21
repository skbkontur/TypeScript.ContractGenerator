using System.Text;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class JavaScriptEnumDeclaration : TypeScriptTypeDeclaration
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            var result = new StringBuilder();
            result.AppendFormat("enum {0} ", Name);
            result.Append(Definition!.GenerateCode(context));
            return result.ToString();
        }
    }
}