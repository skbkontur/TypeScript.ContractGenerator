using System;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TypeScriptGenerationOptions
    {
        public EnumGenerationMode EnumGenerationMode { get; set; } = EnumGenerationMode.TypeScriptEnum;

        public NullabilityMode NullabilityMode { get; set; } = NullabilityMode.Pessimistic;

        public LinterDisableMode LinterDisableMode { get; set; } = LinterDisableMode.EsLint;

        public bool EnableOptionalProperties { get; set; } = true;

        public bool EnableExplicitNullability { get; set; } = true;

        public bool UseGlobalNullable { get; set; }

        [NotNull]
        [Obsolete("Will be removed in 2.0")]
        public Func<string, string> Pluralize { get; set; } = x => x + "s";

        [NotNull]
        public static TypeScriptGenerationOptions Default { get; } = new TypeScriptGenerationOptions();
    }
}