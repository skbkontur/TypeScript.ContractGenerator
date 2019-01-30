namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class GenericContainingRootType
    {
        public GenericChildType<int> GenericIntChild { get; set; }
        public GenericChildType<int?> GenericNullableIntChild { get; set; }
        public GenericChildType<int?>[] ArrayGenericNullableIntChild { get; set; }
        public ChildWithSeveralGenericParameters<string, int> SeveralGenericParameters { get; set; }
        public GenericChildType<ChildWithConstraint<string>> GenericChildType { get; set; }
        public GenericChildType<ChildWithConstraint<GenericChildType<string>>> GenericHell { get; set; }
    }

    public class GenericChildType<T>
    {
        public T ChildType { get; set; }
        public T[] ChildTypes { get; set; }
    }

    public class ChildWithConstraint<T>
        where T : class
    {
        public T Child { get; set; }
    }

    public class ChildWithSeveralGenericParameters<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
    }
}