using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class EndToEndTests : TestBase
    {
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
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, rootType).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(EnumWithConstGetterContainingRootType), "not-annotated-const-getter-typescript-enum")]
        [TestCase(typeof(AnnotatedEnumWithConstGetterContainingRootType), "annotated-const-getter-typescript-enum")]
        public void GenerateEnumWithConstGetterTest(Type type, string expectedFileName)
        {
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, new TestCustomTypeGenerator(), type).Single();
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
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, (ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(MethodRootType), typeof(TestCustomTypeGenerator), "method-class")]
        public void CustomGeneratorWithMethodsTest(Type rootType, Type type, string expectedFileName)
        {
            var options = TypeScriptGenerationOptions.Default;
            var rootTypes = new[] {typeof(MethodRootType), typeof(NullableReferenceMethodType)};
            var customGenerator = new TestCustomTypeGenerator();

            options.NullabilityMode = NullabilityMode.Pessimistic;
            var generatedCode = GenerateCode(options, (ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();

            options.NullabilityMode = NullabilityMode.Optimistic | NullabilityMode.NullableReference;
            generatedCode = GenerateCode(options, (ICustomTypeGenerator)Activator.CreateInstance(type), rootType).Single();
            expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void CustomGeneratorBuilderTest()
        {
            var customGenerator = new CustomTypeGenerator()
                .WithTypeLocation(TypeInfo.From<AnotherCustomType>(), x => "a/b/c")
                .WithTypeRedirect(TypeInfo.From<byte[]>(), "ByteArray", @"DataTypes\ByteArray")
                .WithTypeLocation(TypeInfo.From<HashSet<string>>(), x => "a/b")
                .WithTypeBuildingContext(TypeInfo.From<HashSet<string>>(), x => new CollectionTypeBuildingContext(x));

            var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, customGenerator, new TypesProvider(typeof(ArrayRootType)));
            var units = generator.Generate();
            var code = units.Select(x => x.GenerateCode(new DefaultCodeGenerationContext()).Replace("\r\n", "\n")).ToArray();

            units.Select(x => x.Path).Should().Equal("", "a/b/c");
            var expectedCodeRoot = GetExpectedCode("CustomGenerator/custom-generator-builder");
            var expectedCodeChild = GetExpectedCode("CustomGenerator/custom-generator-builder-child");
            code.Length.Should().Be(2);
            code[0].Diff(expectedCodeRoot).ShouldBeEmpty();
            code[1].Diff(expectedCodeChild).ShouldBeEmpty();
        }
    }
}