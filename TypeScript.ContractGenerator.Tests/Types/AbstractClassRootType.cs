namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class AbstractClassRootType
    {
        public AbstractClass X { get; set; }
    }

    public abstract class AbstractClass
    {
    }

    public class FirstInheritor : AbstractClass
    {
        public int A { get; set; }
    }

    public class SecondInheritor : AbstractClass
    {
        public string A { get; set; }
    }
}