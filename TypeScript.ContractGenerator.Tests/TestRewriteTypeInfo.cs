using System.Collections.Generic;
using System.Linq;

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
    }
}