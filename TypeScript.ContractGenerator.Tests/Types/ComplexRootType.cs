namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.Types
{
    public class ComplexRootType
    {
        public ChildStruct ChildStruct { get; set; }
        public ChildStruct? NullableChildStruct { get; set; }
        public ChildClass ChildClass { get; set; }
    }

    public struct ChildStruct
    {
        public int A { get; set; }
        public string B { get; set; }
    }

    public class ChildClass
    {
        public decimal C { get; set; }
        public RecursiveChildClass RecursiveChildClass { get; set; }
    }

    public class RecursiveChildClass
    {
        public ChildClass ChildClass { get; set; }
        public RecursiveChildClass RChildClass { get; set; }
    }
}