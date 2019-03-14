using SkbKontur.TypeScript.ContractGenerator.Attributes;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class IgnoreRootType
    {
        public int Int { get; set; }

        [ContractGeneratorIgnore]
        public string IgnoredString { get; set; }

        [ContractGeneratorIgnore]
        public Child Child { get; set; }
    }

    public class Child
    {
        public int I { get; set; }
        public string[] Strings { get; set; }
    }
}