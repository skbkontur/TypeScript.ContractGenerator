using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

        public static Compilation GetCompilation(params string[] directories)
        {
            var project = FromDirectory(directories);
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult();
            return compilation.AddReferences(GetMetadataReferences());
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

        public static MetadataReference[] GetMetadataReferences()
        {
            var types = new[] {typeof(object), typeof(Regex), typeof(Enumerable), typeof(ImmutableArray), typeof(ISet<>), typeof(HashSet<>), typeof(FileInfo), typeof(TypeInfo), typeof(RoslynTypeInfo), typeof(ITypeSymbol), typeof(CSharpCompilation)};
            var netstandardLocation = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "netstandard.dll");
            var locations = types.Select(x => x.Assembly.Location).Concat(new[] {netstandardLocation}).Distinct();
            return locations.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x)).ToArray();
        }
    }
}