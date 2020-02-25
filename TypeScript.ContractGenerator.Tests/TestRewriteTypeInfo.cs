using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            var project = AdhocProject.FromDirectory("../../../../AspNetCoreExample.Generator");
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult()
                                     .AddReferences(MetadataReference.CreateFromFile(typeof(SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo).Assembly.Location));
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));

            var root = tree.GetRoot();

            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();
            
        }
    }
}