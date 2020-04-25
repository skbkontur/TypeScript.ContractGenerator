using System;

namespace SkbKontur.TypeScript.ContractGenerator
{
    [Flags]
    public enum NullabilityMode
    {
        None = 0,
        Pessimistic = 1,
        Optimistic = 2,
        NullableReference = 4,
    }
}