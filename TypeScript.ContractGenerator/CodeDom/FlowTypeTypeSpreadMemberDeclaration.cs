namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeTypeSpreadMemberDeclaration : FlowTypeTypeMemberDeclarationBase
    {
        public FlowTypeType Type { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("...{0};", Type.GenerateCode(context));
        }
    }
}