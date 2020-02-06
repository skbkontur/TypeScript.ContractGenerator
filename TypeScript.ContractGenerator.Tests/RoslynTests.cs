using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
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
        [TestCase(typeof(ArrayRootType), "array-types")]
        [TestCase(typeof(NotNullRootType), "notnull-types")]
        [TestCase(typeof(NonDefaultConstructorRootType), "non-default-constructor")]
        [TestCase(typeof(IgnoreRootType), "ignore-type")]
        public void GenerateCodeTest(Type rootType, string expectedFileName)
        {
            var generatedCode = GenerateCode(rootType).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        private static string[] GenerateCode(Type rootType)
        {
            var generator = new TypeScriptGenerator(new TypeScriptGenerationOptions {EnumGenerationMode = EnumGenerationMode.FixedStringsAndDictionary},
                                                    CustomTypeGenerator.Null, new RoslynTypesProvider(rootType.FullName));
            return generator.Generate().Select(x => x.GenerateCode(new DefaultCodeGenerationContext(JavaScriptTypeChecker.TypeScript)).Replace("\r\n", "\n")).ToArray();
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