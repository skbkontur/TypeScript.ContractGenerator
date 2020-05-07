using System;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace AspNetCoreExample.Generator
{
    public class AspNetCoreExampleCustomGenerator : CustomTypeGenerator
    {
        public AspNetCoreExampleCustomGenerator()
        {
            const string modelsNamespace = "AspNetCoreExample.Api.Models";
            WithTypeRedirect(TypeInfo.From<Guid>(), "Guid", @"dataTypes\Guid")
                .WithTypeLocationRule(x => x.FullName.StartsWith(modelsNamespace), x => "dto/" + x.FullName.Substring(modelsNamespace.Length + 1).Replace(".", "/"))
                .WithTypeLocationRule(ApiControllerTypeBuildingContext.Accept, x => $"api/{x.Name}".Replace("Controller", "Api"))
                .WithTypeBuildingContext(ApiControllerTypeBuildingContext.Accept, (unit, type) => new ApiControllerTypeBuildingContext(unit, type));
        }
    }
}