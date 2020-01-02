using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public static class TypeInfo
    {
        public static ITypeInfo FromType<T>()
        {
            return new TypeWrapper(typeof(T));
        }
    }
}