namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTernaryIfExpression : TypeScriptExpression
    {
        public TypeScriptTernaryIfExpression(TypeScriptExpression condition, TypeScriptExpression trueBranch, TypeScriptExpression falseBranch)
        {
            Condition = condition;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public TypeScriptExpression Condition { get; }
        public TypeScriptExpression TrueBranch { get; }
        public TypeScriptExpression FalseBranch { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Condition.GenerateCode(context)} ? {TrueBranch.GenerateCode(context)} : {FalseBranch.GenerateCode(context)}";
        }
    }
}