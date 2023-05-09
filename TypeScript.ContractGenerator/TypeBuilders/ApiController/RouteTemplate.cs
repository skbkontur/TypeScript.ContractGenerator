using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public class RouteTemplate : IEquatable<RouteTemplate>
    {
        public RouteTemplate(string value)
        {
            this.value = value;
            ParameterNames = GetParameterNames(value);
            ValueWithoutConstraints = Regex.Replace(value, @"{(\w+):\w+}", "{$1}");
        }

        public HashSet<string> ParameterNames { get; }

        public string ValueWithoutConstraints { get; }

        public override string ToString() => value;

        private static HashSet<string> GetParameterNames(string routeTemplate)
        {
            return new HashSet<string>(
                Regex
                    .Matches(routeTemplate, @"{(\w+):\w+}")
                    .Cast<Match>()
                    .Where(x => x.Success)
                    .Where(x => x.Groups.Count > 1)
                    .Where(x => x.Groups[1].Success)
                    .Select(x => x.Groups[1].Value));
        }

        public bool Equals(RouteTemplate? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return value == other.value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((RouteTemplate)obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        private readonly string value;
    }
}