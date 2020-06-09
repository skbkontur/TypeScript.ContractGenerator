using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class AssemblyWrapper : IAssemblyInfo
    {
        public AssemblyWrapper(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        public ITypeInfo[] GetTypes()
        {
            return Assembly.GetTypes().Select(TypeInfo.From).ToArray();
        }
    }
}