using System;
using System.IO;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;
using SkbKontur.TypeScript.ContractGenerator.Tests.Helpers;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            var project = new AdhocWorkspace().AddProject(Guid.NewGuid().ToString(), LanguageNames.CSharp);
            project = project.AddDocument("ApiControllerTypeBuildingContext.cs", File.ReadAllText(GetFilePath("ApiControllerTypeBuildingContext.txt")))
                             .Project;

            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult()!
                                     .AddReferences(AdhocProject.GetMetadataReferences())
                                     .AddReferences(MetadataReference.CreateFromFile(typeof(ControllerBase).Assembly.Location));

            TypeInfoRewriter.Types.Clear();
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));
            var result = TypeInfoRewriter.Rewrite(compilation, tree);
            var str = result.ToString();

            var expectedCode = File.ReadAllText(GetFilePath("ApiControllerTypeBuildingContext.Expected.txt")).Replace("\r\n", "\n");

            str.Diff(expectedCode).ShouldBeEmpty();

            var assembly = AdhocProject.CompileAssembly(new[] {result});
            var buildingContext = assembly.GetType("AspNetCoreExample.Generator.ApiControllerTypeBuildingContext")!;
            var acceptMethod = buildingContext.GetMethod("Accept", BindingFlags.Public | BindingFlags.Static)!;

            acceptMethod.Invoke(null, new object[] {TypeInfo.From<bool>()}).Should().Be(false);
            acceptMethod.Invoke(null, new object[] {TypeInfo.From<UsersController>()}).Should().Be(true);
        }

        private static string GetFilePath(string filename)
        {
            return $"{TestContext.CurrentContext.TestDirectory}/Files/{filename}";
        }
    }
}