using System;
using System.IO;

using Microsoft.CodeAnalysis;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Roslyn
{
    public static class AdhocProject
    {
        public static Project FromDirectory(string directory)
        {
            var project = new AdhocWorkspace().AddProject(Guid.NewGuid().ToString(), LanguageNames.CSharp);
            foreach (var path in Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(path);
                project = project.AddDocument(fileInfo.Name, File.ReadAllText(fileInfo.FullName)).Project;
            }

            return project;
        }
    }
}