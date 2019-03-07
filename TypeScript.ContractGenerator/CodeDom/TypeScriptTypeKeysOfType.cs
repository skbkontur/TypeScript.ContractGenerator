namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeKeysOfType : TypeScriptType
    {
        public TypeScriptType TargetType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("$Keys<{0}>", TargetType.GenerateCode(context));
        }
    }
}