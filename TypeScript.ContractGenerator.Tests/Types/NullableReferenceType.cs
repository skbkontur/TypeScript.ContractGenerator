using System;
using System.Collections.Generic;

#nullable enable

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

        // todo (p.vostretsov, 30.12.2019): бажок
        public Dictionary<int, string?> ValueTypeKeyDictionary { get; set; }
        public Dictionary<string[], string?> ComplexDictionary { get; set; }

        // todo (p.vostretsov, 30.12.2019): Not implemented
        public Tuple<string, string, string> Tuple { get; set; }
        public Tuple<string?, string, string> NullableFirstItemTuple { get; set; }
        public Tuple<string, string?, string> NullableSecondItemTuple { get; set; }
        public Tuple<string, string, string?> NullableThirdItemTuple { get; set; }
        public Tuple<string?, string?, string?>? NullableTuple { get; set; }

        public Tuple<string[]?, string?[], string?[]?>? InnerTuples { get; set; }
    }
}