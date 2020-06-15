using System;
using System.Linq;
using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public static class AssemblyUtils
    {
        public static T[] GetImplementations<T>(this Assembly assembly) where T : class
        {
            var interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"Type {interfaceType.Name} must be interface");

            var implementations = assembly.DefinedTypes
                                          .Where(t => t.ImplementedInterfaces.Contains(interfaceType) && !t.IsGenericType && !t.IsAbstract && t.IsPublic)
                                          .ToArray();
            return implementations.Select(CreateInstance<T>).Where(x => x != null).Select(x => x!).ToArray();
        }

        private static T? CreateInstance<T>(TypeInfo typeInfo) where T : class
        {
            var constructorInfo = typeInfo.DeclaredConstructors.SingleOrDefault(c => c.IsPublic && !c.GetParameters().Any());
            if (constructorInfo == null)
                return null;
            return (T)constructorInfo.Invoke(new object[0]);
        }
    }
}