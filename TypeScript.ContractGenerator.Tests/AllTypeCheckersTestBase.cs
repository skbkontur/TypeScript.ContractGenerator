using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    [TestFixture(JavaScriptTypeChecker.Flow)]
    [TestFixture(JavaScriptTypeChecker.TypeScript)]
    public abstract class AllTypeCheckersTestBase : TestBase
    {
        protected AllTypeCheckersTestBase(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }
    }
}