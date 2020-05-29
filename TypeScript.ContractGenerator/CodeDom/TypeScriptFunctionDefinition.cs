using System.Collections.Generic;
using System.Text;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptFunctionDefinition : TypeScriptDefinition
    {
        public List<TypeScriptArgumentDeclaration> Arguments { get; } = new List<TypeScriptArgumentDeclaration>();

        public List<TypeScriptStatement> Body { get; } = new List<TypeScriptStatement>();

        public TypeScriptType Result { get; set; }
        public bool IsAsync { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }

        public override string GenerateCode(string name, ICodeGenerationContext context)
        {
            var result = new StringBuilder();
            if (IsPublic)
            {
                result.Append("public ");
            }
            if (IsStatic)
            {
                result.Append("static ");
            }
            if (IsAsync)
            {
                result.Append("async ");
            }
            result.AppendFormat("{0}({1}): {2} {{", name, Arguments.GenerateCodeCommaSeparated(context), Result.GenerateCode(context)).Append(context.NewLine);
            foreach (var statement in Body)
            {
                result.AppendWithTab(context.Tab, statement.GenerateCode(context), context.NewLine).Append(context.NewLine);
            }
            result.Append("}");
            return result.ToString();
        }
    }
}