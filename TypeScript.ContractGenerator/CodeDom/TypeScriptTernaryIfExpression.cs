namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTernaryIfExpression : TypeScriptExpression
    {
        public TypeScriptExpression Condition { get; set; }
        public TypeScriptExpression True { get; set; }
        public TypeScriptExpression False { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Condition.GenerateCode(context)} ? {True.GenerateCode(context)} : {False.GenerateCode(context)}";
        }
    }
}