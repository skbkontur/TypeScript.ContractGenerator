using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptObjectLiteral : TypeScriptExpression
    {
        public TypeScriptObjectLiteral(IEnumerable<TypeScriptObjectLiteralInitializer> properties)
        {
            this.properties = properties.ToList();
        }

        public List<TypeScriptObjectLiteralInitializer> Properties => properties;

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var result = new StringBuilder();
            result.Append("{").Append(context.NewLine);
            var propertiesString = string.Join("," + context.NewLine, Properties.Select(x => x.GenerateCode(context)));
            if (propertiesString.Length > 0)
                propertiesString += ",";
            result.AppendWithTab(context.Tab, propertiesString, context.NewLine).Append(context.NewLine);
            result.Append("}");
            return result.ToString();
        }

        private readonly List<TypeScriptObjectLiteralInitializer> properties;
    }
}