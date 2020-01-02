using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class TypeInfo
    {
        public static ITypeInfo FromType<T>()
        {
            return new TypeWrapper(typeof(T));
        }
    }
}