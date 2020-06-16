using System.Linq;

using Microsoft.CodeAnalysis;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public static class RoslynCustomizationProvider
    {
        public static (ICustomTypeGenerator, IRootTypesProvider) GetCustomization(string[] directories, string[] assemblies)
        {
            var compilation = AdhocProject.GetCompilation(directories)
                                          .AddReferences(assemblies.Select(x => MetadataReference.CreateFromFile(x)));

            var customGenerationTypes = compilation.GetCustomGenerationTypes();
            var assembly = AdhocProject.CompileAssembly(customGenerationTypes);

            var customTypeGenerator = assembly.GetImplementations<ICustomTypeGenerator>().Single();
            var typesProvider = assembly.GetImplementations<IRootTypesProvider>().Single();

            return (customTypeGenerator, typesProvider);
        }
    }
}