namespace TypeScript.CodeDom
{
    public class FlowTypeClassDeclaration : FlowTypeStatement
    {
        public string Name { get; set; }

        public FlowTypeClassDefinition Defintion { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Defintion.GenerateBody(Name, context) + ";";
        }
    }
}