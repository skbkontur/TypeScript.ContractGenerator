using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptInterfaceFunctionMember : TypeScriptInterfaceMember
    {
        public TypeScriptInterfaceFunctionMember([NotNull] string name, [NotNull] TypeScriptType result,
                                                 [NotNull, ItemNotNull] params TypeScriptArgumentDeclaration[] arguments)
        {
            Name = name;
            Result = result;
            Arguments = new List<TypeScriptArgumentDeclaration>(arguments);
        }

        public string Name { get; }
        public TypeScriptType Result { get; }
        public List<TypeScriptArgumentDeclaration> Arguments { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Name}({Arguments.GenerateCodeCommaSeparated(context)}): {Result.GenerateCode(context)}";
        }
    }
}