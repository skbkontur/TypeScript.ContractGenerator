using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
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
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, rootType).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(MethodRootType), typeof(TestCustomTypeGenerator), NullabilityMode.Pessimistic, "method-class")]
        [TestCase(typeof(MethodRootType), typeof(TestCustomTypeGenerator), NullabilityMode.Pessimistic | NullabilityMode.NullableReference, "method-class")]
        [TestCase(typeof(MethodRootType), typeof(TestCustomTypeGenerator), NullabilityMode.NullableReference, "method-invalid-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), typeof(TestCustomTypeGenerator), NullabilityMode.NullableReference, "method-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), typeof(TestCustomTypeGenerator), NullabilityMode.Optimistic | NullabilityMode.NullableReference, "method-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), typeof(TestCustomTypeGenerator), NullabilityMode.Optimistic, "method-no-nullable-reference-class")]
        public void CustomGeneratorWithMethodsTest(Type rootType, Type type, NullabilityMode nullabilityMode, string expectedFileName)
        {
            var options = TypeScriptGenerationOptions.Default;
            options.NullabilityMode = nullabilityMode;
            var generatedCode = GenerateCode(options, (ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void TestNullableReferences()
        {
            var options = new TypeScriptGenerationOptions {NullabilityMode = NullabilityMode.NullableReference};
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typeof(NullableReferenceType)).Single();
            var expectedCode = GetExpectedCode("Options/nullable-reference");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        private static string[] GenerateCode(TypeScriptGenerationOptions options, ICustomTypeGenerator customTypeGenerator, Type rootType)
        {
            var generator = new TypeScriptGenerator(options, customTypeGenerator, new RoslynTypesProvider(rootType.FullName));
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