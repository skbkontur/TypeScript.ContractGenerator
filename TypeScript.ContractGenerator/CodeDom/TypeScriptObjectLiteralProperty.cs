namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptObjectLiteralProperty : TypeScriptObjectLiteralInitializer
    {
        public TypeScriptExpression Name { get; set; }
        public TypeScriptExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("[{0}]: {1}", Name.GenerateCode(context), Value.GenerateCode(context));
        }
    }
}