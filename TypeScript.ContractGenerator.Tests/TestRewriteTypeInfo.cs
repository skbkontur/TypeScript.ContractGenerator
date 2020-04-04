using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using AspNetCoreExample.Api.Controllers;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            var project = AdhocProject.FromDirectory($"{TestContext.CurrentContext.TestDirectory}/../../../../AspNetCoreExample.Generator");
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult()
                                     .AddReferences(GetMetadataReferences())
                                     .AddReferences(MetadataReference.CreateFromFile(typeof(ControllerBase).Assembly.Location));
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));

            var root = tree.GetRoot();

            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();

            var expectedCode = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/Files/ApiControllerTypeBuildingContext.txt").Replace("\r\n", "\n");

            str.Diff(expectedCode).ShouldBeEmpty();

            var assembly = CompileAssembly(result.SyntaxTree);
            var buildingContext = assembly.GetType("AspNetCoreExample.Generator.ApiControllerTypeBuildingContext");
            var acceptMethod = buildingContext.GetMethod("Accept", BindingFlags.Public | BindingFlags.Static);

            acceptMethod.Invoke(null, new object[] {TypeInfo.From<bool>()}).Should().Be(false);
            acceptMethod.Invoke(null, new object[] {TypeInfo.From<UsersController>()}).Should().Be(true);
        }

        private static Assembly CompileAssembly(SyntaxTree tree)
        {
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("TypeScript.CustomGenerator.Customization", new[] {tree}, GetMetadataReferences(), options);
            var peStream = new MemoryStream();
            var pdbStream = new MemoryStream();
            var emitResult = compilation.Emit(peStream, pdbStream);
            if (!emitResult.Success)
            {
                foreach (var diagnostic in emitResult.Diagnostics)
                    Console.WriteLine(diagnostic);
                Assert.Fail("Failed to compile");
            }

            return Assembly.Load(peStream.ToArray(), pdbStream.ToArray());
        }

        private static MetadataReference[] GetMetadataReferences()
        {
            var types = new[] {typeof(object), typeof(Enumerable), typeof(ISet<>), typeof(HashSet<>), typeof(TypeInfo), typeof(RoslynTypeInfo)};
            var netstandardLocation = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "netstandard.dll");
            var locations = types.Select(x => x.Assembly.Location).Concat(new[] {netstandardLocation}).Distinct();
            return locations.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x)).ToArray();
        }
    }
}