using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class RoslynTests
    {
        [TestCase(typeof(NamingRootType), "type-names")]
        [TestCase(typeof(SimpleRootType), "simple-types")]
        [TestCase(typeof(SimpleNullableRootType), "nullable-types")]
        [TestCase(typeof(EnumContainingRootType), "enum-types")]
        [TestCase(typeof(ComplexRootType), "complex-types")]
        [TestCase(typeof(GenericRootType<>), "generic-root")]
        [TestCase(typeof(GenericContainingRootType), "generic-types")]
#if NET472
        [TestCase(typeof(ArrayRootType), "array-types")]
#endif
        [TestCase(typeof(NotNullRootType), "notnull-types")]
        [TestCase(typeof(NonDefaultConstructorRootType), "non-default-constructor")]
        [TestCase(typeof(IgnoreRootType), "ignore-type")]
        public void GenerateCodeTest(Type rootType, string expectedFileName)
        {
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, rootType).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        [Ignore("p.vostretsov, 25.05.2020: ")]
        public void TestNullableReferences()
        {
            var options = new TypeScriptGenerationOptions {NullabilityMode = NullabilityMode.NullableReference};
            var generatedCode = GenerateCode(options, typeof(NullableReferenceType)).Single();
            var expectedCode = GetExpectedCode("Options/nullable-reference");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        private static string[] GenerateCode(TypeScriptGenerationOptions options, Type rootType)
        {
            var generator = new TypeScriptGenerator(options, CustomTypeGenerator.Null, new RoslynTypesProvider(rootType.FullName));
            return generator.Generate().Select(x => x.GenerateCode(new DefaultCodeGenerationContext()).Replace("\r\n", "\n")).ToArray();
        }

        private static string GetExpectedCode(string expectedCodeFilePath)
        {
            return File.ReadAllText(GetFilePath(expectedCodeFilePath)).Replace("\r\n", "\n");
        }

        private static string GetFilePath(string filename)
        {
            return $"{TestContext.CurrentContext.TestDirectory}/Files/{filename}.ts";
        }
    }
}