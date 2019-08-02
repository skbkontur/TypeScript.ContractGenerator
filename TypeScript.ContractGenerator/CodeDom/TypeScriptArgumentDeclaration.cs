namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArgumentDeclaration : TypeScriptExpression
    {
        public string Name { get; set; }
        public bool Optional { get; set; }
        public TypeScriptType Type { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var optional = Optional ? "?" : "";
            return $"{Name}{optional}: {Type.GenerateCode(context)}";
        }
    }
}