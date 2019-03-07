namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptThisReference : TypeScriptExpression
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "this";
        }
    }
}