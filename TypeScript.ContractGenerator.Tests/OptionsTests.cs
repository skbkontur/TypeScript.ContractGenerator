using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class OptionsTests : AllTypeCheckersTestBase
    {
        public OptionsTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(EnumGenerationMode.FixedStringsAndDictionary, "enum-generation-fixed-strings")]
        public void EnumGenerationModeTest(EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, typeof(DefaultEnum)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

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

        [TestCase("pluralize-default", null)]
        [TestCase("pluralize-custom", "Items")]
        public void PluralizeTest(string expectedFileName, string pluralizeSuffix)
        {
            var options = TypeScriptGenerationOptions.Default;
            if (!string.IsNullOrEmpty(pluralizeSuffix))
                options.Pluralize = s => s + pluralizeSuffix;

            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typeof(EnumContainingRootType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase("nullability-pessimistic", NullabilityMode.Pessimistic)]
        [TestCase("nullability-optimistic", NullabilityMode.Optimistic)]
        public void NullabilityModeTest(string expectedFileName, NullabilityMode mode)
        {
            var options = TypeScriptGenerationOptions.Default;
            options.NullabilityMode = mode;

            var generatedCode = GenerateCode(options, CustomTypeGenerator.Null, typeof(NullabilityModeRootType)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }
    }

    public class OptionsTypeScriptTests : TypeScriptTestBase
    {
        public OptionsTypeScriptTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(EnumGenerationMode.TypeScriptEnum, "enum-generation-typescript-enum")]
        public void EnumGenerationModeTest(EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, typeof(DefaultEnum)).Single();
            var expectedCode = GetExpectedCode($"Options/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }
    }
}