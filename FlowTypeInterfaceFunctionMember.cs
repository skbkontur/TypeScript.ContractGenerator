using System.Collections.Generic;
using System.Linq;

namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeInterfaceFunctionMember
    {
        public string Name { get; set; }
        public List<FlowTypeArgumentDeclaration> Arguments { get { return arguments; } }
        public FlowTypeType Result { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("{0}({1}): {2}", Name, GenerateArgumentListCode(context), Result.GenerateCode(context));
        }

        private string GenerateArgumentListCode(ICodeGenerationContext context)
        {
            return string.Join(", ", Arguments.Select(x => x.GenerateCode(context)));
        }

        private readonly List<FlowTypeArgumentDeclaration> arguments = new List<FlowTypeArgumentDeclaration>();
    }
}