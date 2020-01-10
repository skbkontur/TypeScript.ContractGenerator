using System;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypeScriptGenerationOptions
    {
        public EnumGenerationMode EnumGenerationMode { get; set; } = EnumGenerationMode.TypeScriptEnum;

        public NullabilityMode NullabilityMode { get; set; } = NullabilityMode.Pessimistic;

        public LinterDisableMode LinterDisableMode { get; set; } = LinterDisableMode.EsLint;

        public bool EnableOptionalProperties { get; set; } = true;

        public bool EnableExplicitNullability { get; set; } = true;

        public bool UseGlobalNullable { get; set; }

        [Obsolete("Will be removed in 2.0")]
        public Func<string, string> Pluralize { get; set; } = x => x + "s";

        public static TypeScriptGenerationOptions Default { get; } = new TypeScriptGenerationOptions();
    }
}