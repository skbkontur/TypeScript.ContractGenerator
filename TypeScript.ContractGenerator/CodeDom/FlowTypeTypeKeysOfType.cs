namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeTypeKeysOfType : FlowTypeType
    {
        public FlowTypeType TargetType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("$Keys<{0}>", TargetType.GenerateCode(context));
        }
    }
}