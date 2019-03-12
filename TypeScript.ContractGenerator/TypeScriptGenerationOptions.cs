using System;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TypeScriptGenerationOptions
    {
        public EnumGenerationMode EnumGenerationMode { get; set; } = EnumGenerationMode.FixedStringsAndDictionary;

        public bool EnableOptionalProperties { get; set; } = true;

        public bool EnableExplicitNullability { get; set; } = true;

        public bool UseGlobalNullable { get; set; }

        [NotNull]
        public Func<string, string> Pluralize { get; set; } = x => x + "s";

        [NotNull]
        public static TypeScriptGenerationOptions Default { get; } = new TypeScriptGenerationOptions();
    }
}