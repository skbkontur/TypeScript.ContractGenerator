namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptReturnStatement : TypeScriptStatement
    {
        public TypeScriptReturnStatement()
        {
        }

        public TypeScriptReturnStatement(TypeScriptExpression expression)
        {
            Expression = expression;
        }

        public TypeScriptExpression Expression { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "return " + Expression.GenerateCode(context) + ";";
        }
    }
}