using System.Linq;
using System.Text.RegularExpressions;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public static class RouteTemplateHelper
    {
        public static string GetRouteTemplate(ITypeInfo controller, IMethodInfo action)
        {
            var rawTemplate = FindFullRouteTemplate(controller, action);

            var valueWithoutConstraints = Regex.Replace(rawTemplate, @"{(\w+):\w+}", "{$1}");
            if (valueWithoutConstraints.StartsWith("/"))
                return valueWithoutConstraints;

            return "/" + valueWithoutConstraints;
        }

        private static string FindFullRouteTemplate(ITypeInfo controller, IMethodInfo action)
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
                return Regex.Replace(routeTemplate, "^~/?", "");

            if (string.IsNullOrEmpty(routePrefix))
                return routeTemplate;

            if (string.IsNullOrEmpty(routeTemplate))
                return routePrefix;

            return $"{routePrefix}/{routeTemplate}";
        }
    }
}