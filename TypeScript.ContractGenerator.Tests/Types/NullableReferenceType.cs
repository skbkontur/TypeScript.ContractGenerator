using System;
using System.Collections.Generic;

#nullable enable

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class NullableReferenceType
    {
        public Tuple<string, string?, string?>? NullableTuple { get; set; }
    }
}