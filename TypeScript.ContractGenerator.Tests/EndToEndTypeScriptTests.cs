using System;
using System.Linq;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class EndToEndTypeScriptTests : TypeScriptTestBase
    {
        public EndToEndTypeScriptTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(typeof(EnumWithConstGetterContainingRootType), EnumGenerationMode.TypeScriptEnum, "not-annotated-const-getter-typescript-enum")]
        [TestCase(typeof(AnnotatedEnumWithConstGetterContainingRootType), EnumGenerationMode.TypeScriptEnum, "annotated-const-getter-typescript-enum")]
        public void GenerateEnumWithConstGetterTest(Type type, EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, new TestCustomTypeGenerator(), type).Single();
            var expectedCode = GetExpectedCode($"Enums/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }
    }
}