namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class JavaScriptEnumMember
    {
        public string Name { get; set; }
        public FlowTypeExpression ValueLiteral { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            if (ValueLiteral == null)
            {
                return Name;
            }
            return $"{Name} = {ValueLiteral.GenerateCode(context)}";
        }

    }
}