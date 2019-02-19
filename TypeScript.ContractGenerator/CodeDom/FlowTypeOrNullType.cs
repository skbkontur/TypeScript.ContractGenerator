namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeOrNullType : FlowTypeUnionType
    {
        public FlowTypeOrNullType(FlowTypeType innerType)
            : base(new[]
                {
                    new FlowTypeBuildInType("null"),
                    innerType
                })
        {
        }
    }
}