namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeOfValue : TypeScriptType
    {
        public TypeScriptExpression TargetValue { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"typeof {TargetValue.GenerateCode(context)}";
        }
    }
}