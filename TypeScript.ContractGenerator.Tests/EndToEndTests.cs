using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
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
            var generatedCode = GenerateCode(rootType).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(EnumWithConstGetterContainingRootType), EnumGenerationMode.FixedStringsAndDictionary, "not-annotated-const-getter-fixed-strings")]
        [TestCase(typeof(AnnotatedEnumWithConstGetterContainingRootType), EnumGenerationMode.FixedStringsAndDictionary, "annotated-const-getter-fixed-strings")]
        public void GenerateEnumWithConstGetterTest(Type type, EnumGenerationMode enumGenerationMode, string expectedFileName)
        {
            var generatedCode = GenerateCode(new TypeScriptGenerationOptions {EnumGenerationMode = enumGenerationMode}, CustomTypeGenerator.Null, type).Single();
            var expectedCode = GetExpectedCode($"Enums/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
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
        [TestCase(typeof(FirstInheritor), typeof(TestCustomTypeGenerator), "inherit-abstract-class")]
        [TestCase(typeof(AbstractClassRootType), typeof(TestCustomTypeGenerator), "abstract-class")]
        public void CustomGeneratorTest(Type rootType, Type type, string expectedFileName)
        {
            var generatedCode = GenerateCode((ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void CustomGeneratorBuilderTest()
        {
            var customGenerator = new CustomTypeGenerator()
                .WithTypeLocation<AnotherCustomType>(x => "a/b/c")
                .WithTypeRedirect<byte[]>("ByteArray", @"DataTypes\ByteArray")
                .WithTypeBuildingContext<HashSet<string>>(x => new CollectionTypeBuildingContext(x));

            var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, customGenerator, new RootTypesProvider(typeof(ArrayRootType)));
            var units = generator.Generate();
            var code = units.Select(x => x.GenerateCode(new DefaultCodeGenerationContext(JavaScriptTypeChecker)).Replace("\r\n", "\n")).ToArray();

            units.Select(x => x.Path).Should().Equal("", "a/b/c");
            var expectedCodeRoot = GetExpectedCode("CustomGenerator/custom-generator-builder");
            var expectedCodeChild = GetExpectedCode("CustomGenerator/custom-generator-builder-child");
            code.Length.Should().Be(2);
            code[0].Diff(expectedCodeRoot).ShouldBeEmpty();
            code[1].Diff(expectedCodeChild).ShouldBeEmpty();
        }
    }
}