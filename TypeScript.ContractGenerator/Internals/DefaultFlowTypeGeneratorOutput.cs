using System.Collections.Generic;
using System.Linq;

namespace TypeScript.ContractGenerator.Internals
{
    internal class DefaultFlowTypeGeneratorOutput : IFlowTypeUnitFactory
    {
        public FlowTypeUnit[] Units { get { return units.Values.ToArray(); } }

        public FlowTypeUnit GetOrCreateTypeUnit(string path)
        {
            FlowTypeUnit result;
            if (units.TryGetValue(path, out result))
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