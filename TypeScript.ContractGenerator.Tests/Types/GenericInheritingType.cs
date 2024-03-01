using System;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class GenericInheritingType<TType>
        where TType : Enum
    {
        public TType Type { get; set; }
    }
}