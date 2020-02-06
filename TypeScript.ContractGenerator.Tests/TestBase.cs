using System;
using System.IO;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    [TestFixture(JavaScriptTypeChecker.Flow)]
    [TestFixture(JavaScriptTypeChecker.TypeScript)]
    public abstract class TestBase
    {
        protected TestBase(JavaScriptTypeChecker javaScriptTypeChecker)
        {
            filesGenerationContext = FilesGenerationContext.Create(javaScriptTypeChecker, LinterDisableMode.TsLint);
        }

        protected string[] GenerateCode(Type rootType)
        {
            return GenerateCode(CustomTypeGenerator.Null, rootType);
        }

        protected string[] GenerateCode(ICustomTypeGenerator customTypeGenerator, Type rootType)
        {
            return GenerateCode(TestOptions, customTypeGenerator, rootType);
        }

        protected string[] GenerateCode(TypeScriptGenerationOptions options, ICustomTypeGenerator customTypeGenerator, Type rootType)
        {
            var generator = new TypeScriptGenerator(options, customTypeGenerator, new TypesProvider(rootType));
            if (JavaScriptTypeChecker == JavaScriptTypeChecker.Flow && options.EnumGenerationMode == EnumGenerationMode.TypeScriptEnum)
                throw new ArgumentException("Invalid EnumGenerationMode for JavaScriptTypeChecker.Flow");
            return generator.Generate().Select(x => x.GenerateCode(new DefaultCodeGenerationContext(JavaScriptTypeChecker)).Replace("\r\n", "\n")).ToArray();
        }

        protected void GenerateFiles(ICustomTypeGenerator customTypeGenerator, string folderName, params Type[] rootTypes)
        {
            var path = $"{TestContext.CurrentContext.TestDirectory}/{folderName}/{JavaScriptTypeChecker}";
            if (Directory.Exists(path))
                Directory.Delete(path, recursive : true);
            Directory.CreateDirectory(path);

            var generator = new TypeScriptGenerator(TestOptions, customTypeGenerator, new TypesProvider(rootTypes));
            generator.GenerateFiles(path, JavaScriptTypeChecker);
        }

        protected void CheckDirectoriesEquivalence(string expectedDirectory, string actualDirectory)
        {
            expectedDirectory = $"{TestContext.CurrentContext.TestDirectory}/{expectedDirectory}/{JavaScriptTypeChecker}";
            actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/{actualDirectory}/{JavaScriptTypeChecker}";

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

        protected JavaScriptTypeChecker JavaScriptTypeChecker => filesGenerationContext.JavaScriptTypeChecker;

        protected TypeScriptGenerationOptions TestOptions => new TypeScriptGenerationOptions
            {
                EnumGenerationMode = EnumGenerationMode.FixedStringsAndDictionary,
                LinterDisableMode = LinterDisableMode.TsLint,
            };

        private readonly FilesGenerationContext filesGenerationContext;
    }
}