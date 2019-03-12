using System;
using System.Diagnostics.CodeAnalysis;

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
            var options = new TypeScriptGenerationOptions {EnumGenerationMode = EnumGenerationMode.TypeScriptEnum};
            var generator = new TypeScriptGenerator(options, CustomTypeGenerator.Null, RootTypesProvider.Default);
            Action filesGeneration = () => generator.GenerateFiles("", JavaScriptTypeChecker.Flow);
            filesGeneration.Should().Throw<ArgumentException>().And.Message.Should().Be("Flow is not compatible with TypeScript enums");
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void PluralizationValidationTest()
        {
            Assert.Throws<ArgumentException>(() => TryGenerate(null));
            Assert.Throws<ArgumentException>(() => TryGenerate(x => null));
            Assert.Throws<ArgumentException>(() => TryGenerate(x => x));
            Assert.DoesNotThrow(() => TryGenerate(x => x + "s"));
            Assert.DoesNotThrow(() => TryGenerate(x => x + "Items"));
        }

        private static void TryGenerate(Func<string, string> pluralize)
        {
            var options = TypeScriptGenerationOptions.Default;
            options.Pluralize = pluralize;
            var generator = new TypeScriptGenerator(options, CustomTypeGenerator.Null, RootTypesProvider.Default);
            generator.Generate();
        }
    }
}