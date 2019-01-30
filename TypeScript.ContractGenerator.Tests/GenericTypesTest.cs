using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
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