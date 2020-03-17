using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            var project = AdhocProject.FromDirectory($"{TestContext.CurrentContext.TestDirectory}/../../../../AspNetCoreExample.Generator");
            var types = new[] {typeof(object), typeof(HashSet<>), typeof(Internals.TypeInfo)};
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult().AddReferences(types.Select(x => MetadataReference.CreateFromFile(x.Assembly.Location)));
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));

            var root = tree.GetRoot();

            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();
        }

        [Test]
        public void ApiControllerTypeBuildingContextRewrite()
        {
            var expectedCode = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/Files/ApiControllerTypeBuildingContext.txt").Replace("\r\n", "\n");

            var project = AdhocProject.FromDirectory($"{TestContext.CurrentContext.TestDirectory}/../../../../AspNetCoreExample.Generator");
            var types = new[] {typeof(object), typeof(HashSet<>), typeof(Internals.TypeInfo)};
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult().AddReferences(types.Select(x => MetadataReference.CreateFromFile(x.Assembly.Location)));
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));
            var root = tree.GetRoot();
            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();
            
            str.Diff(expectedCode).ShouldBeEmpty();
        }
    }
}