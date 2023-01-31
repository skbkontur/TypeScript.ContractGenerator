using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
using SkbKontur.TypeScript.ContractGenerator.Tests.Helpers;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    [TestFixture(typeof(RootTypesProvider))]
    [TestFixture(typeof(RoslynTypesProvider))]
    public class EndToEndTests<TTypesProvider> : TestBase
        where TTypesProvider : IRootTypesProvider
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
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(null, rootType);
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, customGenerator, typesProvider).Single();
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(EnumWithConstGetterContainingRootType), "not-annotated-const-getter-typescript-enum")]
        [TestCase(typeof(AnnotatedEnumWithConstGetterContainingRootType), "annotated-const-getter-typescript-enum")]
        public void GenerateEnumWithConstGetterTest(Type type, string expectedFileName)
        {
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(typeof(TestCustomTypeGenerator), type);
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, customGenerator, typesProvider).Single();
            var expectedCode = GetExpectedCode($"Enums/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(FlatTypeLocator))]
        [TestCase(typeof(SimpleStructureTypeLocator))]
        public void GenerateFilesTest(Type type)
        {
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(type, typeof(CommonUsingRootType), typeof(CommonUsingRootType2), typeof(CommonUsingRootType3));
            GenerateFiles(customGenerator, $"{type.Name}.Actual", typesProvider);
            CheckDirectoriesEquivalence($"Files/{type.Name}.Expected", $"{type.Name}.Actual");
        }

        [Test]
        public void GenerateFilesWithCustomContentMarker()
        {
            const string marker = "My custom content marker";
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(typeof(FlatTypeLocator), typeof(CommonUsingRootType));
            GenerateFiles(customGenerator, "Actual", typesProvider, marker);
            CheckDirectoriesEquivalence("Files/WithProjectIds", "Actual", marker);
        }

        [TestCase(typeof(SimpleRootType), typeof(TestCustomTypeGenerator), "simple-types")]
        [TestCase(typeof(SimpleNullableRootType), typeof(TestCustomTypeGenerator), "nullable-types")]
        [TestCase(typeof(ArrayRootType), typeof(TestCustomTypeGenerator), "array-types")]
        [TestCase(typeof(EnumWithConstGetterContainingRootType), typeof(TestCustomPropertyResolver), "custom-property-resolver")]
        [TestCase(typeof(FirstInheritor), typeof(TestCustomTypeGenerator), "inherit-abstract-class")]
        [TestCase(typeof(AbstractClassRootType), typeof(TestCustomTypeGenerator), "abstract-class")]
        public void CustomGeneratorTest(Type rootType, Type type, string expectedFileName)
        {
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(type, rootType);
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, customGenerator, typesProvider).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [TestCase(typeof(MethodRootType), NullabilityMode.Pessimistic, "method-pessimistic-class")]
        [TestCase(typeof(MethodRootType), NullabilityMode.Pessimistic | NullabilityMode.NullableReference, "method-pessimistic-class")]
        [TestCase(typeof(MethodRootType), NullabilityMode.Optimistic | NullabilityMode.NullableReference, "method-optimistic-reference-class")]
        [TestCase(typeof(MethodRootType), NullabilityMode.NullableReference, "method-invalid-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), NullabilityMode.NullableReference, "method-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), NullabilityMode.Optimistic | NullabilityMode.NullableReference, "method-nullable-reference-class")]
        [TestCase(typeof(NullableReferenceMethodType), NullabilityMode.Optimistic, "method-no-nullable-reference-class")]
        public void CustomGeneratorWithMethodsTest(Type rootType, NullabilityMode nullabilityMode, string expectedFileName)
        {
            var options = TypeScriptGenerationOptions.Default;
            options.NullabilityMode = nullabilityMode;
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(typeof(TestCustomTypeGenerator), rootType);
            var generatedCode = GenerateCode(options, customGenerator, typesProvider).Single();
            var expectedCode = GetExpectedCode($"CustomGenerator/{expectedFileName}");
            generatedCode.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void CustomGeneratorBuilderTest()
        {
            var (customGenerator, typesProvider) = GetCustomization<TTypesProvider>(typeof(BuilderCustomGenerator), typeof(ArrayRootType));
            var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, customGenerator, typesProvider);
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