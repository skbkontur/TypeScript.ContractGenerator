using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class DefaultFlowTypeGeneratorOutput : IFlowTypeUnitFactory
    {
        public FlowTypeUnit[] Units => units.Values.ToArray();

        public FlowTypeUnit GetOrCreateTypeUnit(string path)
        {
            if (units.TryGetValue(path, out var result))
                return result;
            result = new FlowTypeUnit
                {
                    Path = path,
                };
            units.Add(path, result);
            return result;
        }

        private readonly Dictionary<string, FlowTypeUnit> units = new Dictionary<string, FlowTypeUnit>();
    }
}