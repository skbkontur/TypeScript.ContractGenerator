using System.Linq;
using System.Text.RegularExpressions;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public static class RouteTemplateHelper
    {
        public static RouteTemplate? FindFullRouteTemplate(ITypeInfo controller, IMethodInfo action)
        {
            var routePrefix = (controller
                               .GetAttributes(inherit : true)
                               .FirstOrDefault(x => x.HasName(KnownTypeNames.Attributes.RoutePrefix)
                                                    || x.HasName(KnownTypeNames.Attributes.Route))
                               ?.GetValue("Template", string.Empty) ?? string.Empty)
                .Replace("[controller]", controller.Name.Replace("Controller", ""));

            var routeTemplate = (action.GetAttributes(inherit : false)
                                       .Where(x => x.HasName(KnownTypeNames.Attributes.Route)
                                                   || KnownTypeNames.HttpAttributeNames.Any(x.HasName))
                                       .Select(x => x.GetValue("Template", ""))
                                       .SingleOrDefault(x => !string.IsNullOrEmpty(x)) ?? string.Empty)
                                .Replace("[controller]", controller.Name.Replace("Controller", ""))
                                .Replace("[action]", action.Name);

            if (routeTemplate.StartsWith("~/"))
                return new RouteTemplate(Regex.Replace(routeTemplate, "^~/?", ""));

            if (string.IsNullOrEmpty(routePrefix))
                return new RouteTemplate(routeTemplate);

            if (string.IsNullOrEmpty(routeTemplate))
                return new RouteTemplate(routePrefix);

            return new RouteTemplate($"{routePrefix}/{routeTemplate}");
        }
    }
}