using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public static class RoslynGenerator
    {
        public static void Generate(Options options)
        {
            var project = AdhocProject.FromDirectory(options.Directory.ToArray());

            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult()
                                     .AddReferences(AdhocProject.GetMetadataReferences())
                                     .AddReferences(options.Assembly.Select(x => MetadataReference.CreateFromFile(x)));

            var customGenerationTypes = AdhocProject.GetCustomGenerationTypes(compilation);
            var assembly = AdhocProject.CompileAssembly(customGenerationTypes);

            var customTypeGenerator = assembly.GetImplementations<ICustomTypeGenerator>().Single();
            var typesProvider = assembly.GetImplementations<IRootTypesProvider>().Single();

            var typeGenerator = new TypeScriptGenerator(options.ToTypeScriptGenerationOptions(), customTypeGenerator, typesProvider);
            typeGenerator.GenerateFiles(options.OutputDirectory);
        }
    }
}