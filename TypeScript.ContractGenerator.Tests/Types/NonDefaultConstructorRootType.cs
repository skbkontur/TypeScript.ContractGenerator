namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class NonDefaultConstructorRootType
    {
        public NonDefaultConstructorRootType(string s)
        {
            S = s;
        }

        public string S { get; set; }
        public NonDefaultConstructorChildType Child { get; set; }
    }

    public class NonDefaultConstructorChildType
    {
        public NonDefaultConstructorChildType(int i)
        {
            I = i;
        }

        public int I { get; set; }
    }
}