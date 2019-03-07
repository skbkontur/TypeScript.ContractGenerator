namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeOfValue : TypeScriptType
    {
        public TypeScriptExpression TargetValue { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("typeof {0}", TargetValue.GenerateCode(context));
        }
    }
}