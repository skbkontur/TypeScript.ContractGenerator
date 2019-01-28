namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.Types
{
    public class CommonUsingRootType
    {
        public CommonType[] CommonTypes { get; set; }
    }

    public class CommonUsingRootType2
    {
        public CommonType CommonType { get; set; }
    }

    public class CommonUsingRootType3
    {
        public CommonType AnotherCommonType { get; set; }
        public CommonUsingRootType CommonsContainingType { get; set; }
    }
}