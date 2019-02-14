using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class CustomTypeGenerator
    {
        public static ICustomTypeGenerator Null => new NullCustomTypeGenerator();
    }
}