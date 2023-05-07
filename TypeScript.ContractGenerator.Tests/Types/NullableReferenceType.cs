#nullable enable

using System;
using System.Collections.Generic;

#pragma warning disable 8618, 8714

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class NullableReferenceType
    {
        public string String { get; set; }
        public string? NullableString { get; set; }

        public string[] Array { get; set; }
        public string?[] NullableStringArray { get; set; }
        public string[]? NullableArrayOfStrings { get; set; }
        public string?[]? NullableArrayOfNullableStrings { get; set; }

        public List<string> List { get; set; }
        public List<string?>? NullableList { get; set; }

        public Dictionary<string, string> Dictionary { get; set; }
        public Dictionary<string?, string> DictionaryWithNullableKey { get; set; }
        public Dictionary<string, string?> DictionaryWithNullableValue { get; set; }
        public Dictionary<string?, string?>? NullableDictionary { get; set; }
        public Dictionary<int, string?> ValueTypeKeyDictionary { get; set; }
        public Dictionary<string[], string?> ComplexDictionary { get; set; }

        public Tuple<string, string, string> Tuple { get; set; }
        public Tuple<string?, string, string> NullableFirstItemTuple { get; set; }
        public Tuple<string, string?, string> NullableSecondItemTuple { get; set; }
        public Tuple<string, string, string?> NullableThirdItemTuple { get; set; }
        public Tuple<string?, string?, string?>? NullableTuple { get; set; }
        public Tuple<string[]?, string?[], string?[]?>? NullableTupleInnerArrays { get; set; }

        public NullableReferenceTypeWithContext DifferentContext { get; set; }
    }

    public class NullableReferenceTypeWithContext
    {
        public string NotNullFirst { get; set; }
        public string? First { get; set; }
        public string?[]?[]? Second { get; set; }
        public Tuple<string, Tuple<string?, int?, int>?, string?> Third { get; set; }
    }
}