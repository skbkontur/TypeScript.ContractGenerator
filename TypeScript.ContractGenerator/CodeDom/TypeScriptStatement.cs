namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public abstract class TypeScriptStatement
    {
        public abstract string GenerateCode(ICodeGenerationContext context);
    }
}