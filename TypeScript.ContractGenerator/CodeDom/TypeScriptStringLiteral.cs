using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptStringLiteral : TypeScriptExpression
    {
        public TypeScriptStringLiteral([NotNull] string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"'{Value}'";
        }
    }
}