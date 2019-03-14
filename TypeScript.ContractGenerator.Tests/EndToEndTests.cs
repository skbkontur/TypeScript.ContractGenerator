using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class EndToEndTests : AllTypeCheckersTestBase
    {
        public EndToEndTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

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
            var generatedCode = GenerateCode(rootType).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }

        [TestCase(typeof(EnumWithConstGetterContainingRootType), EnumGenerationMode.FixedStringsAndDictionary, "not-annotated-const-getter-fixed-strings")]
        [TestCase(typeof(AnnotatedEnumWithConstGetterContainingRootType), EnumGenerationMode.FixedStringsAndDictionary, "annotated-const-getter-fixed-strings")]
        public void GenerateEnumWithConstGetterTest(Type type, EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, type).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"Enums/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }

        [TestCase(typeof(FlatTypeLocator))]
        [TestCase(typeof(SimpleStructureTypeLocator))]
        public void GenerateFilesTest(Type type)
        {
            var rootTypes = new[] {typeof(CommonUsingRootType), typeof(CommonUsingRootType2), typeof(CommonUsingRootType3)};
            GenerateFiles((ICustomTypeGenerator)Activator.CreateInstance(type), $"{type.Name}.Actual", rootTypes);
            CheckDirectoriesEquivalence($"Files/{type.Name}.Expected", $"{type.Name}.Actual");
        }

        [TestCase(typeof(SimpleRootType), typeof(TestCustomTypeGenerator), "simple-types")]
        [TestCase(typeof(SimpleNullableRootType), typeof(TestCustomTypeGenerator), "nullable-types")]
        [TestCase(typeof(ArrayRootType), typeof(TestCustomTypeGenerator), "array-types")]
        [TestCase(typeof(EnumWithConstGetterContainingRootType), typeof(TestCustomPropertyResolver), "custom-property-resolver")]
        public void CustomGeneratorTest(Type rootType, Type type, string expectedFileName)
        {
            var generatedCode = GenerateCode((ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }
    }

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
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, type).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"Enums/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }
    }
}