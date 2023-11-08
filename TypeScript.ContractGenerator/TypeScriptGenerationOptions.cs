namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypeScriptGenerationOptions
    {
        public NullabilityMode NullabilityMode { get; set; } = NullabilityMode.Pessimistic;
        public LinterDisableMode LinterDisableMode { get; set; } = LinterDisableMode.EsLint;
        public bool EnableOptionalProperties { get; set; } = true;
        public bool UseGlobalNullable { get; set; }
        public string? CustomContentMarker { get; set; }
        public bool UseTypeImports { get; set; } = false;
        public static TypeScriptGenerationOptions Default => new TypeScriptGenerationOptions();
    }
}