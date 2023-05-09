using System.Collections.Generic;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public static class KnownTypeNames
    {
        public const string ActionResultOfT = "ActionResult`1";
        public const string ActionResult = "ActionResult";

        public static readonly HashSet<string> HttpAttributeNames = new HashSet<string>
            {
                Attributes.HttpGet,
                Attributes.HttpPost,
                Attributes.HttpPut,
                Attributes.HttpDelete,
                Attributes.HttpPatch
            };

        public static class Attributes
        {
            public const string Route = "Route";
            public const string RoutePrefix = "RoutePrefix";
            public const string HttpGet = "HttpGet";
            public const string HttpPost = "HttpPost";
            public const string HttpPut = "HttpPut";
            public const string HttpPatch = "HttpPatch";
            public const string HttpDelete = "HttpDelete";
            public const string FromBody = "FromBody";
        }
    }
}