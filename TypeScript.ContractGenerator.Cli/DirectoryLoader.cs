using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public class DirectoryAssemblyLoadContext : AssemblyLoadContext
    {
        public DirectoryAssemblyLoadContext(string mainAssemblyToLoadPath)
            : base(true)
        {
            resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
        }

        protected override Assembly Load(AssemblyName name)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => AssemblyNameEqual(name, a));
            if (assembly != null)
                return assembly;
            var assemblyPath = resolver.ResolveAssemblyToPath(name);
            return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        private static bool AssemblyNameEqual(AssemblyName name, Assembly a)
        {
            return a.FullName.Split(',')[0].Equals(name.Name, StringComparison.OrdinalIgnoreCase);
        }

        private readonly AssemblyDependencyResolver resolver;
    }
}