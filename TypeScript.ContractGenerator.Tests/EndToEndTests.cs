using System;
using System.IO;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using TypeScript.CodeDom;
using TypeScript.ContractGenerator.Tests.CustomTypeGenerators;
using TypeScript.ContractGenerator.Tests.Types;

namespace TypeScript.ContractGenerator.Tests
{
    public class EndToEndTests : FlowTypeTestBase
    {
        public EndToEndTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(typeof(NamingRootType), "type-names.expected")]
        [TestCase(typeof(SimpleRootType), "simple-types.expected")]
        [TestCase(typeof(SimpleNullableRootType), "nullable-types.expected")]
        [TestCase(typeof(EnumContainingRootType), "enum-types.expected")]
        [TestCase(typeof(ComplexRootType), "complex-types.expected")]
        [TestCase(typeof(GenericRootType<>), "generic-root.expected")]
        [TestCase(typeof(GenericContainingRootType), "generic-types.expected")]
        [TestCase(typeof(ArrayRootType), "array-types.expected")]
        [TestCase(typeof(NotNullRootType), "notnull-types.expected")]
        public void GenerateCodeTest(Type rootType, string expectedFileName)
        {
            var generatedCode = GenerateCode(rootType).Single();
            var expectedCode = File.ReadAllText(GetFilePath($"SimpleGenerator/{expectedFileName}")).Replace("\r\n", "\n");
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

        [TestCase(typeof(SimpleRootType), "simple-types.expected")]
        [TestCase(typeof(SimpleNullableRootType), "nullable-types.expected")]
        [TestCase(typeof(ArrayRootType), "array-types.expected")]
        public void CustomGeneratorTest(Type rootType, string expectedFileName)
        {
            var generatedCode = GenerateCode(new CustomTypeGenerator(), rootType).Single();
            var expectedCode = File.ReadAllText(GetFilePath($"CustomGenerator/{expectedFileName}")).Replace("\r\n", "\n");
            generatedCode.Should().Be(expectedCode);
        }
    }
}