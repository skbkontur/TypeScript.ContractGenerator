using System;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class GenericInheritingType<TType>
        where TType : Enum
    {
        [NotNull]
        public TType Type { get; set; }
    }
}