using System;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class OptionsValidationTests
    {
        [Test]
        public void FlowAndTypeScriptEnumIncompatibilityTest()
        {
            var options = new FlowTypeGenerationOptions {EnumGenerationMode = EnumGenerationMode.TypeScriptEnum};
            var generator = new FlowTypeGenerator(options, CustomTypeGenerator.Null, RootTypesProvider.Default);
            Action filesGeneration = () => generator.GenerateFiles("", JavaScriptTypeChecker.Flow);
            filesGeneration.Should().Throw<ArgumentException>().And.Message.Should().Be("Flow is not compatible with TypeScript enums");
        }
    }
}