namespace SkbKontur.TypeScript.ContractGenerator
{
    public class FlowTypeGenerationOptions
    {
        public EnumGenerationMode EnumGenerationMode { get; set; } = EnumGenerationMode.FixedStringsAndDictionary;

        public bool EnableOptionalProperties { get; set; } = true;

        public bool EnableExplicitNullability { get; set; } = true;

        public static FlowTypeGenerationOptions Default { get; } = new FlowTypeGenerationOptions();
    }
}