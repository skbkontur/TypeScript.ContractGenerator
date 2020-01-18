using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class OptionsTests : TestBase
    {
        [TestCase(true, "optional-properties-enabled")]
        [TestCase(false, "optional-properties-disabled")]
        public void OptionalPropertiesTest(bool optionalPropertiesEnabled, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnableOptionalProperties = optionalPropertiesEnabled}, CustomTypeGenerator.Null, typeof(SingleNullablePropertyType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(true, "explicit-nullability-enabled")]
        [TestCase(false, "explicit-nullability-disabled")]
        public void ExplicitNullabilityTest(bool explicitNullabilityEnabled, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnableExplicitNullability = explicitNullabilityEnabled}, CustomTypeGenerator.Null, typeof(ExplicitNullabilityRootType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(true, "global-nullable-enabled")]
        [TestCase(false, "global-nullable-disabled")]
        public void GlobalNullableTest(bool useGlobalNullable, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {UseGlobalNullable = useGlobalNullable}, CustomTypeGenerator.Null, typeof(GlobalNullableRootType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase("nullability-pessimistic", NullabilityMode.Pessimistic)]
        [TestCase("nullability-optimistic", NullabilityMode.Optimistic)]
        public void NullabilityModeTest(string expectedFileName, NullabilityMode mode)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {NullabilityMode = mode}, CustomTypeGenerator.Null, typeof(NullabilityModeRootType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
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
    }
}