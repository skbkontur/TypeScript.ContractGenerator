namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public abstract class TypeScriptExpression
    {
        public abstract string GenerateCode(ICodeGenerationContext context);
    }
}