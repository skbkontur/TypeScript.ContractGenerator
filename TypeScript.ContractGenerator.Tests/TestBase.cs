using System;
using System.IO;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            filesGenerationContext = FilesGenerationContext.Create(LinterDisableMode.TsLint);
        }

        protected string[] GenerateCode(TypeScriptGenerationOptions options, ICustomTypeGenerator customTypeGenerator, Type rootType)
        {
            var generator = new TypeScriptGenerator(options, customTypeGenerator, new TypesProvider(rootType));
            return generator.Generate().Select(x => x.GenerateCode(new DefaultCodeGenerationContext()).Replace("\r\n", "\n")).ToArray();
        }

        protected void GenerateFiles(ICustomTypeGenerator customTypeGenerator, string folderName, params Type[] rootTypes)
        {
            var path = $"{TestContext.CurrentContext.TestDirectory}/{folderName}";
            if (Directory.Exists(path))
                Directory.Delete(path, recursive : true);
            Directory.CreateDirectory(path);

            var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, customTypeGenerator, new TypesProvider(rootTypes));
            generator.GenerateFiles(path);
        }

        protected void CheckDirectoriesEquivalence(string expectedDirectory, string actualDirectory)
        {
            expectedDirectory = $"{TestContext.CurrentContext.TestDirectory}/{expectedDirectory}";
            actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/{actualDirectory}";

            CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory);
        }

        public static void CheckDirectoriesEquivalenceInner(string expectedDirectory, string actualDirectory, bool generatedOnly = false)
        {
            if (!generatedOnly && (!Directory.Exists(expectedDirectory) || !Directory.Exists(actualDirectory)))
                Assert.Fail("Both directories should exist");

            const string marker = "// TypeScriptContractGenerator's generated content";
            var expectedDirectoryFiles = new string[0];
            var actualDirectoryFiles = new string[0];
            if (Directory.Exists(expectedDirectory))
                expectedDirectoryFiles = Directory.EnumerateFiles(expectedDirectory).Where(x => !generatedOnly || File.ReadAllText(x).Contains(marker)).ToArray();
            if (Directory.Exists(actualDirectory))
                actualDirectoryFiles = Directory.EnumerateFiles(actualDirectory).Where(x => !generatedOnly || File.ReadAllText(x).Contains(marker)).ToArray();

            var expectedFiles = expectedDirectoryFiles.Select(Path.GetFileName).ToArray();
            var actualFiles = actualDirectoryFiles.Select(Path.GetFileName).ToArray();

            actualFiles.Should().BeEquivalentTo(expectedFiles);

            foreach (var filename in expectedFiles)
            {
                var expected = File.ReadAllText($"{expectedDirectory}/{filename}").Replace("\r\n", "\n");
                var actual = File.ReadAllText($"{actualDirectory}/{filename}").Replace("\r\n", "\n");
                actual.Diff(expected).ShouldBeEmpty();
            }

            var expectedDirectories = new string[0];
            var actualDirectories = new string[0];
            if (Directory.Exists(expectedDirectory))
                expectedDirectories = Directory.EnumerateDirectories(expectedDirectory).Select(Path.GetFileName).ToArray();
            if (Directory.Exists(actualDirectory))
                actualDirectories = Directory.EnumerateDirectories(actualDirectory).Select(Path.GetFileName).ToArray();

            if (!generatedOnly)
                actualDirectories.Should().BeEquivalentTo(expectedDirectories);

            foreach (var directory in expectedDirectories)
                CheckDirectoriesEquivalenceInner($"{expectedDirectory}/{directory}", $"{actualDirectory}/{directory}", generatedOnly);
        }

        protected string GetExpectedCode(string expectedCodeFilePath)
        {
            return File.ReadAllText(GetFilePath(expectedCodeFilePath)).Replace("\r\n", "\n");
        }

        private string GetFilePath(string filename)
        {
            return $"{TestContext.CurrentContext.TestDirectory}/Files/{filename}.{filesGenerationContext.FileExtension}";
        }

        private readonly FilesGenerationContext filesGenerationContext;
    }
}