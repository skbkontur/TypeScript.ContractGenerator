using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class FlowTypeGenerationOptions
    {
        public EnumGenerationMode EnumGenerationMode { get; set; } = EnumGenerationMode.FixedStringsAndDictionary;

        public bool EnableOptionalProperties { get; set; } = true;

        public bool EnableExplicitNullability { get; set; } = true;

        [NotNull]
        public static FlowTypeGenerationOptions Default { get; } = new FlowTypeGenerationOptions();
    }
}