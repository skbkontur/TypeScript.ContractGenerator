using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TypeScript.CodeDom;
using TypeScript.ContractGenerator.Internals;

namespace TypeScript.ContractGenerator.Tests
{
    [TestFixture(JavaScriptTypeChecker.Flow)]
    [TestFixture(JavaScriptTypeChecker.TypeScript)]
    public abstract class FlowTypeTestBase
    {
        protected FlowTypeTestBase(JavaScriptTypeChecker javaScriptTypeChecker)
        {
            this.javaScriptTypeChecker = javaScriptTypeChecker;
            fileExtension = javaScriptTypeChecker == JavaScriptTypeChecker.TypeScript ? "ts" : "js";
        }

        protected string[] GenerateCode(Type rootType)
        {
            return GenerateCode(null, rootType);
        }

        protected string[] GenerateCode(ICustomTypeGenerator customTypeGenerator, Type rootType)
        {
            var generator = new FlowTypeGenerator(customTypeGenerator, new[] {rootType});
            return generator.Generate().Select(x => x.GenerateCode(new DefaultCodeGenerationContext(javaScriptTypeChecker))).ToArray();
        }

        protected void GenerateFiles(ICustomTypeGenerator customTypeGenerator, string folderName, params Type[] rootTypes)
        {
            var path = $"{TestContext.CurrentContext.TestDirectory}/{folderName}/{javaScriptTypeChecker}";
            if (Directory.Exists(path))
                Directory.Delete(path, recursive : true);
            Directory.CreateDirectory(path);

            var generator = new FlowTypeGenerator(customTypeGenerator, rootTypes);
            if (javaScriptTypeChecker == JavaScriptTypeChecker.Flow)
                generator.GenerateFiles(path);
            else
                generator.GenerateTypeScriptFiles(path);
        }

        protected void CheckDirectoriesEquivalence(string expectedDirectory, string actualDirectory)
        {
            expectedDirectory = $"{TestContext.CurrentContext.TestDirectory}/{expectedDirectory}/{javaScriptTypeChecker}";
            actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/{actualDirectory}/{javaScriptTypeChecker}";

            CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory);
        }

        private static void CheckDirectoriesEquivalenceInner(string expectedDirectory, string actualDirectory)
        {
            if (!Directory.Exists(expectedDirectory) || !Directory.Exists(actualDirectory))
                Assert.Fail("Both directories should exist");

            var expectedFiles = Directory.EnumerateFiles(expectedDirectory).Select(Path.GetFileName).ToArray();
            var actualFiles = Directory.EnumerateFiles(actualDirectory).Select(Path.GetFileName).ToArray();

            actualFiles.Should().BeEquivalentTo(expectedFiles);

            foreach (var filename in expectedFiles)
                File.ReadAllText($"{actualDirectory}/{filename}").Should().Be(File.ReadAllText($"{expectedDirectory}/{filename}"));

            var expectedDirectories = Directory.EnumerateDirectories(expectedDirectory).Select(Path.GetFileName).ToArray();
            var actualDirectories = Directory.EnumerateDirectories(actualDirectory).Select(Path.GetFileName).ToArray();

            actualDirectories.Should().BeEquivalentTo(expectedDirectories);

            foreach (var directory in expectedDirectories)
                CheckDirectoriesEquivalenceInner($"{expectedDirectory}/{directory}", $"{actualDirectory}/{directory}");
        }

        protected string GetFilePath(string filename)
        {
            return $"{TestContext.CurrentContext.TestDirectory}/Files/{filename}.{fileExtension}";
        }

        private readonly JavaScriptTypeChecker javaScriptTypeChecker;
        private readonly string fileExtension;
    }
}