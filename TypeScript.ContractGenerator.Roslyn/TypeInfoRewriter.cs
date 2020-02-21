using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class TypeInfoRewriter : CSharpSyntaxRewriter
    {
        public TypeInfoRewriter(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override SyntaxNode? VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var @namespace = node.Name.ToString();
            if (@namespace != "System" && !@namespace.StartsWith("System.") && !@namespace.StartsWith("SkbKontur.TypeScript.ContractGenerator"))
                return null;

            return base.VisitUsingDirective(node);
        }

        public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                var typeInfo = semanticModel.GetTypeInfo(memberAccess.Expression);
                Types.Add(RoslynTypeInfo.From(typeInfo.Type));
                //if (typeInfo.Type.MetadataName == "SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo")
                //{
                //}
            }

            return base.VisitInvocationExpression(node);
        }

        private readonly SemanticModel semanticModel;

        public static List<ITypeInfo> Types = new List<ITypeInfo>();
    }
}