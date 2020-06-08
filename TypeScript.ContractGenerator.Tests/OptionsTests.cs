using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Tests.Helpers;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    [TestFixture(typeof(RootTypesProvider))]
    [TestFixture(typeof(RoslynTypesProvider))]
    public class OptionsTests<TTypesProvider> : TestBase
        where TTypesProvider : IRootTypesProvider
    {
        [TestCase(true, "optional-properties-enabled")]
        [TestCase(false, "optional-properties-disabled")]
        public void OptionalPropertiesTest(bool optionalPropertiesEnabled, string expectedFileName)
        {
            var options = new TypeScriptGenerationOptions {EnableOptionalProperties = optionalPropertiesEnabled};
            var typesProvider = GetTypesProvider<TTypesProvider>(typeof(SingleNullablePropertyType));
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typesProvider).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(NullabilityMode.Pessimistic, "explicit-nullability-enabled")]
        [TestCase(NullabilityMode.None, "explicit-nullability-disabled")]
        public void ExplicitNullabilityTest(NullabilityMode nullabilityMode, string expectedFileName)
        {
            var options = new TypeScriptGenerationOptions {NullabilityMode = nullabilityMode};
            var typesProvider = GetTypesProvider<TTypesProvider>(typeof(ExplicitNullabilityRootType));
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typesProvider).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(true, "global-nullable-enabled")]
        [TestCase(false, "global-nullable-disabled")]
        public void GlobalNullableTest(bool useGlobalNullable, string expectedFileName)
        {
            var options = new TypeScriptGenerationOptions {UseGlobalNullable = useGlobalNullable};
            var typesProvider = GetTypesProvider<TTypesProvider>(typeof(GlobalNullableRootType));
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typesProvider).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase("nullability-pessimistic", NullabilityMode.Pessimistic)]
        [TestCase("nullability-optimistic", NullabilityMode.Optimistic)]
        public void NullabilityModeTest(string expectedFileName, NullabilityMode mode)
        {
            var options = new TypeScriptGenerationOptions {NullabilityMode = mode};
            var typesProvider = GetTypesProvider<TTypesProvider>(typeof(NullabilityModeRootType));
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typesProvider).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void TestNullableReferences()
        {
            var options = new TypeScriptGenerationOptions {NullabilityMode = NullabilityMode.NullableReference};
            var typesProvider = GetTypesProvider<TTypesProvider>(typeof(NullableReferenceType));
            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typesProvider).Single();
            var expectedCode = GetExpectedCode("Options/nullable-reference");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }
    }
}