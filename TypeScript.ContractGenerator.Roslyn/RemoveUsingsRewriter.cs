using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RemoveUsingsRewriter : CSharpSyntaxRewriter
    {
        public RemoveUsingsRewriter(SemanticModel semanticModel)
        {
            var diagnostics = semanticModel.GetDiagnostics();
            unnecessaryNodes = new HashSet<TextSpan>();
            foreach (var diagnostic in diagnostics.Where(d => d.Id == "CS8019"))
                unnecessaryNodes.Add(diagnostic.Location.SourceSpan);
        }

        public override SyntaxNode? VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (unnecessaryNodes.Contains(node.Span))
                return null;

            return base.VisitUsingDirective(node);
        }

        private readonly HashSet<TextSpan> unnecessaryNodes;
    }
}