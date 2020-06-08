using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Attributes;
using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Helpers
{
    public class RoslynTypesProvider : IRootTypesProvider
    {
        public RoslynTypesProvider(params Type[] rootTypes)
        {
            this.rootTypes = rootTypes;
            project = AdhocProject.FromDirectory(TestContext.CurrentContext.TestDirectory + "/../../../Types");
            compilation = project.GetCompilationAsync().GetAwaiter().GetResult();
            var coreTypes = new[] {typeof(object), typeof(HashSet<>), typeof(ContractGeneratorIgnoreAttribute)};
            var assemblies = coreTypes.Select(x => x.Assembly.Location).ToArray();
            compilation = compilation.AddReferences(assemblies.Select(x => MetadataReference.CreateFromFile(x)));
            foreach (var diagnostic in compilation.GetDiagnostics())
                File.AppendAllLines("diagnostic.log", new[] {diagnostic.ToString()});
        }

        public ITypeInfo[] GetRootTypes()
        {
            return rootTypes.Select(x => RoslynTypeInfo.From(compilation.GetTypeByMetadataName(x.FullName))).ToArray();
        }

        private readonly Project project;
        private readonly Compilation compilation;
        private readonly Type[] rootTypes;
    }
}