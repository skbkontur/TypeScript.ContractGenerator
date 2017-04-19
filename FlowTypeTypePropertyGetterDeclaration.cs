namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeTypePropertyGetterDeclaration : FlowTypeTypeMemberDeclarationBase
    {
        public FlowTypeArgumentDeclaration Argument { get; set; }
        public FlowTypeType ResultType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("[{0}]: {1};", Argument.GenerateCode(context), ResultType.GenerateCode(context));
        }
    }
}