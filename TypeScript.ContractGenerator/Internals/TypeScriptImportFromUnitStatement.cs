using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class TypeScriptImportFromUnitStatement : TypeScriptImportStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            var typeKeyword = UseTypeKeyword ? "type " : "";
            return $"import {typeKeyword}{{ {TypeName} }} from '{context.GetReferenceFromUnitToAnother(CurrentUnit.Path, TargetUnit.Path)}';";
        }

        public string TypeName { get; set; }
        public TypeScriptUnit TargetUnit { get; set; }
        public TypeScriptUnit CurrentUnit { get; set; }
        public bool UseTypeKeyword { get; set; }
    }
}