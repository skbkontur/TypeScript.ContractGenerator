using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Cli.Utils
{
    internal static class AssemblyUtils
    {
        public static Assembly[] GetAssemblies(string assemblyName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(baseDirectory))
                return null;
            var assemblyFileName = Path.GetFileName(assemblyName);
            var allFoundAssemblies = Directory.EnumerateFiles(baseDirectory, "*", SearchOption.TopDirectoryOnly)
                                              .Where(IsAssemblyFile);

            return allFoundAssemblies.Where(f => Path.GetFileNameWithoutExtension(f).Equals(assemblyFileName, StringComparison.InvariantCultureIgnoreCase) 
                                                 || Path.GetFileName(f).Equals(assemblyFileName, StringComparison.InvariantCultureIgnoreCase))
                                     .Select(Assembly.LoadFrom)
                                     .ToArray();
        }

        private static bool IsAssemblyFile(string fileName)
        {
            var extension = Path.GetExtension(fileName) ?? "";
            return extension.Equals(".dll", StringComparison.InvariantCultureIgnoreCase)
                   || extension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase);
        }

        public static T[] GetImplementations<T>(this Assembly assembly) where T : class
        {
            var interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"Type {interfaceType.Name} must be interface");

            var implementations = assembly.DefinedTypes
                                          .Where(t => t.ImplementedInterfaces.Contains(interfaceType) && !t.IsGenericType && !t.IsAbstract && t.IsPublic)
                                          .ToArray();
            return implementations.Select(CreateInstance<T>).Where(i => i != null).ToArray();
        }

        private static T CreateInstance<T>(TypeInfo typeInfo) where T : class
        {
            var constructorInfo = typeInfo.DeclaredConstructors.SingleOrDefault(c => c.IsPublic && !c.GetParameters().Any());
            if (constructorInfo == null)
                return null;
            return (T)constructorInfo.Invoke(new object[0]);
        }
    }
}