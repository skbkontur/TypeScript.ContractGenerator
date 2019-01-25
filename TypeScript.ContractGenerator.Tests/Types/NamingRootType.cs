namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.Types
{
    public class NamingRootType
    {
        public AllCaps AllCaps { get; set; }
        public SmallFirstLetter SmallFirstLetter { get; set; }
        public SmallSecondLetter SmallSecondLetter { get; set; }
        public SmallThirdLetter SmallThirdLetter { get; set; }
        public SmallFourthLetter SmallFourthLetter { get; set; }
        public string Abc { get; set; }
        public string AbCd { get; set; }
        public string AbcD { get; set; }
        public string ABcd { get; set; }
        public int B { get; set; }
        public int? Nb { get; set; }
        public string MySQLType { get; set; }
    }

    public class AllCaps
    {
        public string A { get; set; }
        public string AB { get; set; }
        public string ABC { get; set; }
        public string ABCD { get; set; }
        public string ABCDE { get; set; }
    }

    public class SmallFirstLetter
    {
        public string a { get; set; }
        public string aB { get; set; }
        public string aBC { get; set; }
        public string aBCD { get; set; }
        public string aBCDE { get; set; }
    }

    public class SmallSecondLetter
    {
        public string Ab { get; set; }
        public string AbC { get; set; }
        public string AbCD { get; set; }
        public string AbCDE { get; set; }
    }

    public class SmallThirdLetter
    {
        public string ABc { get; set; }
        public string ABcD { get; set; }
        public string ABcDE { get; set; }
    }

    public class SmallFourthLetter
    {
        public string ABCd { get; set; }
        public string ABCdE { get; set; }
    }
}