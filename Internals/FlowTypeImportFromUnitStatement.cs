using SKBKontur.Catalogue.FlowType.CodeDom;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.Internals
{
    internal class FlowTypeImportFromUnitStatement : FlowTypeImportStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("import type {{ {0} }} from '{1}';", TypeName, context.GetReferenceFromUnitToAnother(CurrentUnit.Path, TargetUnit.Path));
        }

        public string TypeName { get; set; }
        public FlowTypeUnit TargetUnit { get; set; }
        public FlowTypeUnit CurrentUnit { get; set; }
    }
}