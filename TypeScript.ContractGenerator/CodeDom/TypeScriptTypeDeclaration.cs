namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeDeclaration : TypeScriptStatement
    {
        public string Name { get; set; }
        public TypeScriptType Definition { get; set; }
        public string[] GenericTypeArguments { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var genericArgs = string.Join(", ", GenericTypeArguments ?? new string[0]);
            genericArgs = string.IsNullOrEmpty(genericArgs) ? string.Empty : $"<{genericArgs}>";
            return $"type {Name}{genericArgs} = {Definition.GenerateCode(context)};";
        }
    }
}