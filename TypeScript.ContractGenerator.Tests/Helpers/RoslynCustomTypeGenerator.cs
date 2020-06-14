using System;
using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Helpers
{
    public static class RoslynCustomTypeGenerator
    {
        public static ICustomTypeGenerator GetCustomTypeGenerator(Compilation compilation, Type type)
        {
            var types = compilation.GetNamespaceTypes(x => x.Equals<TestCustomTypeGenerator>());
            var assembly = AdhocProject.CompileAssembly(types);
            return (ICustomTypeGenerator)Activator.CreateInstance(assembly.GetTypes().Single(x => x.FullName == type.FullName))!;
        }
    }
}