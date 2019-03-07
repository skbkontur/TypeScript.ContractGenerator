using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class DefaultTypeScriptGeneratorOutput : ITypeScriptUnitFactory
    {
        public TypeScriptUnit[] Units => units.Values.ToArray();

        public TypeScriptUnit GetOrCreateTypeUnit(string path)
        {
            if (units.TryGetValue(path, out var result))
                return result;
            result = new TypeScriptUnit
                {
                    Path = path,
                };
            units.Add(path, result);
            return result;
        }

        private readonly Dictionary<string, TypeScriptUnit> units = new Dictionary<string, TypeScriptUnit>();
    }
}