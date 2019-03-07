namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptVariableReference : TypeScriptExpression
    {
        public TypeScriptVariableReference(string name)
        {
            this.name = name;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return name;
        }

        private readonly string name;
    }
}