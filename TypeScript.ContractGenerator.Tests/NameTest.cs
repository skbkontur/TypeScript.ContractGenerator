using FluentAssertions;
using NUnit.Framework;
using TypeScript.ContractGenerator.Extensions;

namespace TypeScript.ContractGenerator.Tests
{
    public class NameTest
    {
        [Test]
        public void Test()
        {
            "ASyncMDNs".ToLowerCamelCase().Should().Be("aSyncMDNs");
            "URILocator".ToLowerCamelCase().Should().Be("uriLocator");
            "AbIRabc".ToLowerCamelCase().Should().Be("abIRabc");
            "MySQLType".ToLowerCamelCase().Should().Be("mySQLType");
            "XMLHttpRequest".ToLowerCamelCase().Should().Be("xmlHttpRequest");
            "LaURL".ToLowerCamelCase().Should().Be("laURL");
        }
    }
}