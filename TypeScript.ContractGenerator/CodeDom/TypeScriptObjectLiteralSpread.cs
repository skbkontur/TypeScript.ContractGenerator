namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptObjectLiteralSpread : TypeScriptObjectLiteralInitializer
    {
        public TypeScriptObjectLiteralSpread(TypeScriptVariableReference expression)
        {
            Expression = expression;
        }

        public TypeScriptVariableReference Expression { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("...{0}", Expression.GenerateCode(context));
        }
    }
}