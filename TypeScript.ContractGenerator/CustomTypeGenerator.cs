using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class CustomTypeGenerator
    {
        [NotNull]
        public static ICustomTypeGenerator Null => new NullCustomTypeGenerator();
    }
}