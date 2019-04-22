using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptFunctionCallExpression : TypeScriptExpression
    {
        public TypeScriptFunctionCallExpression(TypeScriptExpression methodName, params TypeScriptExpression[] arguments)
        {
            MethodName = methodName;
            Arguments.AddRange(arguments);
        }

        public TypeScriptFunctionCallExpression(TypeScriptExpression methodName, TypeScriptType[] typeParameters, params TypeScriptExpression[] arguments)
        {
            MethodName = methodName;
            TypeParameters.AddRange(typeParameters);
            Arguments.AddRange(arguments);
        }
        
        public TypeScriptExpression MethodName { get; set; }
        public List<TypeScriptType> TypeParameters => typeParameters;
        public List<TypeScriptExpression> Arguments => arguments;

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return TypeParameters.Count == 0
                       ? string.Format("{0}({1})", MethodName.GenerateCode(context), string.Join(", ", Arguments.Select(x => x.GenerateCode(context))))
                       : string.Format("{0}<{1}>({2})", MethodName.GenerateCode(context),
                                       string.Join(", ", TypeParameters.Select(x => x.GenerateCode(context))),
                                       string.Join(", ", Arguments.Select(x => x.GenerateCode(context))));
        }

        private readonly List<TypeScriptExpression> arguments = new List<TypeScriptExpression>();
        private readonly List<TypeScriptType> typeParameters = new List<TypeScriptType>();
    }
}