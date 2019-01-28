using System.Collections.Generic;

namespace TypeScript.ContractGenerator.Tests.Types
{
    public class ArrayRootType
    {
        public int[] Ints { get; set; }
        public int?[] NullableInts { get; set; }
        public byte[] ByteArray { get; set; }
        public byte?[] NullableByteArray { get; set; }
        public AnotherEnum[] Enums { get; set; }
        public AnotherEnum?[] NullableEnums { get; set; }
        public string[] Strings { get; set; }
        public AnotherCustomType[] CustomTypes { get; set; }
        public List<string> StringsList { get; set; }
        public Dictionary<string, AnotherCustomType> CustomTypesDict { get; set; }
    }

    public enum AnotherEnum
    {
        B,
        C,
    }

    public class AnotherCustomType
    {
        public int D { get; set; }
    }
}