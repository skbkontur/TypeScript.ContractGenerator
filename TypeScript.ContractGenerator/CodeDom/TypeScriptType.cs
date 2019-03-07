namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public abstract class TypeScriptType
    {
        public abstract string GenerateCode(ICodeGenerationContext context);
    }
}