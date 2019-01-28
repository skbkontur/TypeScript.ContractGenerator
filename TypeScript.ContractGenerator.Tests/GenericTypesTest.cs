using FluentAssertions;
using NUnit.Framework;
using TypeScript.ContractGenerator.Tests.Types;

namespace TypeScript.ContractGenerator.Tests
{
    public class GenericTypesTest
    {
        [Test]
        public void RootCantBeGenericType()
        {
            new FlowTypeGenerator(null, new[] {typeof(GenericRootType<CustomType>)}).Generate().Should().BeEmpty();
        }
    }
}