using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class OptionsTests : FlowTypeTestBase
    {
        public OptionsTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(EnumGenerationMode.FixedStringsAndDictionary, "enum-generation-fixed-strings")]
        [TestCase(EnumGenerationMode.TypeScriptEnum, "enum-generation-typescript-enum")]
        public void EnumGenerationModeTest(EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new FlowTypeGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, typeof(DefaultEnum)).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"Options.Expected/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }

        [TestCase(true, "optional-properties-enabled")]
        [TestCase(false, "optional-properties-disabled")]
        public void OptionalPropertiesTest(bool optionalPropertiesEnabled, string expectedFileName)
        {
            var generatedCode = GenerateCode(new FlowTypeGenerationOptions {EnableOptionalProperties = optionalPropertiesEnabled}, CustomTypeGenerator.Null, typeof(SingleNullablePropertyType)).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"Options.Expected/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }

        [TestCase(true, "explicit-nullability-enabled")]
        [TestCase(false, "explicit-nullability-disabled")]
        public void ExplicitNullabilityTest(bool explicitNullabilityEnabled, string expectedFileName)
        {
            var generatedCode = GenerateCode(new FlowTypeGenerationOptions {EnableExplicitNullability = explicitNullabilityEnabled}, CustomTypeGenerator.Null, typeof(NotNullRootType)).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"Options.Expected/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }
    }
}