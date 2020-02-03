using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Attributes;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynTypesProvider : ITypesProvider
    {
        public RoslynTypesProvider(string typeName)
        {
            this.typeName = typeName;
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
            var rootType = compilation.GetTypeByMetadataName(typeName);
            return new[] {RoslynTypeInfo.From(rootType)};
        }

        public ITypeInfo[] GetAssemblyTypes(ITypeInfo type)
        {
            throw new NotImplementedException();
        }

        private readonly string typeName;
        private readonly Project project;
        private readonly Compilation compilation;
    }
}