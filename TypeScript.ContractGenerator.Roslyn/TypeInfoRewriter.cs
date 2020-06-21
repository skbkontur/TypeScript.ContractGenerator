using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class TypeInfoRewriter : CSharpSyntaxRewriter
    {
        private TypeInfoRewriter(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public static SyntaxTree Rewrite(Compilation compilation, SyntaxTree tree)
        {
            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(tree.GetRoot());
            compilation = compilation.ReplaceSyntaxTree(tree, result.SyntaxTree);
            return new RemoveUsingsRewriter(compilation.GetSemanticModel(result.SyntaxTree)).Visit(result).SyntaxTree;
        }

        public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var symbol = semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol == null || !(symbol is IMethodSymbol methodSymbol))
                return base.VisitInvocationExpression(node);

            if (methodSymbol.ContainingType?.ToString() != typeInfoName || methodSymbol.Name != methodName)
                return base.VisitInvocationExpression(node);

            var foundType = GetSingleType(node.Expression, node.ArgumentList);
            var foundTypeSymbol = semanticModel.GetTypeInfo(foundType).Type;
            if (foundTypeSymbol != null)
                Types.Add(RoslynTypeInfo.From(foundTypeSymbol));
            return ArrayElement(Types.Count - 1);
        }

        private static SyntaxNode ArrayElement(int index)
        {
            var identifier = GetIdentifier(thisName);
            var field = SyntaxFactory.IdentifierName(nameof(Types));

            var memberAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, identifier, field);
            var argument = SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(index)));
            return SyntaxFactory.ElementAccessExpression(memberAccess, SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(new[] {argument})));
        }

        private static NameSyntax GetIdentifier(string fullyQualifiedName)
        {
            var dot = fullyQualifiedName.LastIndexOf(".", StringComparison.InvariantCulture);
            if (dot == -1)
                return SyntaxFactory.IdentifierName(fullyQualifiedName);

            return SyntaxFactory.QualifiedName(GetIdentifier(fullyQualifiedName.Substring(0, dot)),
                                               SyntaxFactory.IdentifierName(fullyQualifiedName.Substring(dot + 1)));
        }

        private static TypeSyntax GetSingleType(ExpressionSyntax expression, BaseArgumentListSyntax argumentListSyntax)
        {
            if (expression is GenericNameSyntax name)
                return name.TypeArgumentList.Arguments.Single();

            if (expression is MemberAccessExpressionSyntax memberAccess && memberAccess.Name is GenericNameSyntax genericNameSyntax)
                return genericNameSyntax.TypeArgumentList.Arguments.Single();

            var argument = argumentListSyntax.Arguments.Single().Expression;
            if (argument is TypeOfExpressionSyntax typeofExpression)
                return typeofExpression.Type;

            throw new InvalidOperationException($"Expected either TypeInfo.From<T>() or TypeInfo.From(typeof(T)), but found: {argumentListSyntax.Parent}");
        }

        private readonly SemanticModel semanticModel;

        private static readonly string typeInfoName = typeof(TypeInfo).FullName;
        private static readonly string methodName = nameof(TypeInfo.From);
        private static readonly string thisName = typeof(TypeInfoRewriter).FullName;

        public static readonly List<ITypeInfo> Types = new List<ITypeInfo>();
    }
}