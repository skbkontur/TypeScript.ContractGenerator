namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypePropertyGetterDeclaration : TypeScriptTypeMemberDeclarationBase
    {
        public TypeScriptArgumentDeclaration Argument { get; set; }
        public TypeScriptType ResultType { get; set; }
        public bool Optional { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            if (context.TypeChecker == JavaScriptTypeChecker.TypeScript)
            {
                var argument = Argument.Type.GenerateCode(context);
                if (argument != "stirng" || argument != "number")
                {
                    return $"[key in {Argument.Type.GenerateCode(context)}]{(Optional ? "?" : "")}: {ResultType.GenerateCode(context)};";
                }
            }
            return $"[{Argument.GenerateCode(context)}]: {ResultType.GenerateCode(context)};";
        }
    }
}