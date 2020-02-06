using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;

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
                project = project.AddDocument(fileInfo.Name, File.ReadAllText(fileInfo.FullName)).Project;
            }

            return project;
        }

        public static IEnumerable<INamedTypeSymbol> GetAllTypes(Compilation compilation) =>
            GetAllTypes(compilation.GlobalNamespace);

        private static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol @namespace)
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