using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

namespace AspNetCoreExample.Api.TypeScriptConfiguration;

public class CustomGenerator : CustomTypeGenerator
{
    public CustomGenerator()
    {
        var controllerBase = TypeInfo.From<ControllerBase>();
        WithTypeLocationRule(t => controllerBase.IsAssignableFrom(t), t => $"Api/{t.Name.Replace("Controller", "Api")}")
            .WithTypeLocationRule(t => !controllerBase.IsAssignableFrom(t), t => $"DataTypes/{t.Name}")
            .WithTypeRedirect(TypeInfo.From<Guid>(), "Guid", @"DataTypes\Guid")
            .WithTypeRedirect(TypeInfo.From<DateOnly>(), "DateOnly", @"DataTypes\DateOnly")
            .WithTypeBuildingContext(t => controllerBase.IsAssignableFrom(t), (u, t) => new ApiControllerTypeBuildingContext(u, t));
    }
}