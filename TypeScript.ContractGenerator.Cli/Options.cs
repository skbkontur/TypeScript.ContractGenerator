using CommandLine;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public class Options
    {
        [Option('a', "assembly", Required = true, HelpText = "Assembly to search for `ICustomTypeGenerator` and `IRootTypesProvider` implementations.")]
        public string Assembly { get; set; }

        [Option('o', "outputDir", Required = false, HelpText = "Generated files output directory.", Default = "src")]
        public string OutputDirectory { get; set; }

        [Option('l', "language", Required = false, HelpText = "Generated files language: JavaScript or TypeScript.", Default = JavaScriptTypeChecker.TypeScript)]
        public JavaScriptTypeChecker Language { get; set; }

        [Option("enumMode", Required = false, HelpText = "Enum's generation mode: TypeScript enums or Fixed strings with dictionary ", Default = EnumGenerationMode.TypeScriptEnum)]
        public EnumGenerationMode EnumGenerationMode { get; set; }

        [Option("optionalProps", Required = false, HelpText = "Enables optional properties.", Default = false)]
        public bool EnableOptionalProperties { get; set; }

        [Option("explicitNull", Required = false, HelpText = "Enables explicit nullability", Default = true)]
        public bool EnableExplicitNullability { get; set; }

        [Option("nullabilityMode", Required = false, HelpText = "Nullability mode can set: Pessimistic, Optimistic or NullableReference", Default = NullabilityMode.Optimistic)]
        public NullabilityMode NullabilityMode { get; set; }

        [Option("globalNullable", Required = false, HelpText = "Enables global nullable", Default = true)]
        public bool UseGlobalNullable { get; set; }
    }
}