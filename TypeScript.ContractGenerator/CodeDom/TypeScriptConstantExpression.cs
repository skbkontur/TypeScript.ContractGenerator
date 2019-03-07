namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstantExpression : TypeScriptExpression
    {
        public TypeScriptConstantExpression(string value)
        {
            this.value = value;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return value;
        }

        private readonly string value;
    }
}