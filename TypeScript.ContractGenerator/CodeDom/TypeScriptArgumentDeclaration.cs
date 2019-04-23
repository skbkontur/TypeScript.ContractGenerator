namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArgumentDeclaration : TypeScriptExpression
    {
        public string Name { get; set; }
        public TypeScriptType Type { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Name}: {Type.GenerateCode(context)}";
        }
    }
}