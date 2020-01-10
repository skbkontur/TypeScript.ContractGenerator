using System;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public enum EnumGenerationMode
    {
        [Obsolete("Will be removed in 2.0")]
        FixedStringsAndDictionary = 0,

        TypeScriptEnum = 1,
    }
}