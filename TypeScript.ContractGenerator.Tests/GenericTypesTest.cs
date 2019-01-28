using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.FlowType.ContractGenerator;
using SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.Types;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests
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