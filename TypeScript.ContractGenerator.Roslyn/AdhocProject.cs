using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public static class AdhocProject
    {
        public static Project FromDirectory(params string[] directories)
        {
            var project = new AdhocWorkspace().AddProject(Guid.NewGuid().ToString(), LanguageNames.CSharp);
            var files = directories.SelectMany(d => Directory.EnumerateFiles(d, "*.cs", SearchOption.AllDirectories)).ToArray();
            foreach (var path in files)
            {
                var fileInfo = new FileInfo(path);
                var text = File.ReadAllText(fileInfo.FullName);

                project = project.AddDocument(fileInfo.Name, SourceText.From(text, Encoding.UTF8)).Project;
            }

            return project;
        }

        public static async Task<Compilation> GetCompilationAsync(params string[] directories)
        {
            var project = FromDirectory(directories);
            var compilation = await project.GetCompilationAsync().ConfigureAwait(false);
            return compilation.AddReferences(GetMetadataReferences());
        }

        public static SyntaxTree[] GetCustomGenerationTypes(Compilation compilation)
        {
            return GetNamespaceTypes(compilation, x => !Equals(x, typeof(RootTypesProvider)) &&
                                                       x.Interfaces.Any(i => Equals(i, typeof(IRootTypesProvider))));
        }

        public static SyntaxTree[] GetNamespaceTypes(Compilation compilation, Func<ITypeSymbol, bool> func)
        {
            var providerType = GetAllTypes(compilation).Single(func);

            return providerType.ContainingNamespace.Locations
                               .Select(x => Rewrite(compilation, x.SourceTree))
                               .ToArray();
        }

        public static SyntaxTree Rewrite(Compilation compilation, SyntaxTree tree)
        {
            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(tree.GetRoot());
            compilation = compilation.ReplaceSyntaxTree(tree, result.SyntaxTree);
            return new RemoveUsingsRewriter(compilation.GetSemanticModel(result.SyntaxTree)).Visit(result).SyntaxTree;
        }

        public static bool Equals(ITypeSymbol typeSymbol, Type type)
        {
            return typeSymbol.Name == type.Name && typeSymbol.ContainingNamespace?.ToString() == type.Namespace;
        }

        public static MetadataReference[] GetMetadataReferences()
        {
            var types = new[] {typeof(object), typeof(Enumerable), typeof(ImmutableArray), typeof(ISet<>), typeof(HashSet<>), typeof(FileInfo), typeof(TypeInfo), typeof(RoslynTypeInfo), typeof(ITypeSymbol), typeof(CSharpCompilation)};
            var netstandardLocation = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "netstandard.dll");
            var locations = types.Select(x => x.Assembly.Location).Concat(new[] {netstandardLocation}).Distinct();
            return locations.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x)).ToArray();
        }

        public static Assembly CompileAssembly(SyntaxTree[] tree)
        {
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("TypeScript.CustomGenerator.Customization", tree, GetMetadataReferences(), options);
            var peStream = new MemoryStream();
            var pdbStream = new MemoryStream();
            var emitResult = compilation.Emit(peStream, pdbStream);
            if (!emitResult.Success)
            {
                foreach (var diagnostic in emitResult.Diagnostics)
                    Console.WriteLine(diagnostic);
                throw new InvalidOperationException("Failed to compile");
            }

            return Assembly.Load(peStream.ToArray(), pdbStream.ToArray());
        }

        public static IEnumerable<INamedTypeSymbol> GetAllTypes(Compilation compilation) =>
            GetAllTypes(compilation.GlobalNamespace);

        public static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol @namespace)
        {
            foreach (var type in @namespace.GetTypeMembers())
                foreach (var nestedType in GetNestedTypes(type))
                    yield return nestedType;

            foreach (var nestedNamespace in @namespace.GetNamespaceMembers())
                foreach (var type in GetAllTypes(nestedNamespace))
                    yield return type;
        }

        private static IEnumerable<INamedTypeSymbol> GetNestedTypes(INamedTypeSymbol type)
        {
            yield return type;
            foreach (var nestedType in type.GetTypeMembers().SelectMany(GetNestedTypes))
                yield return nestedType;
        }
    }
}