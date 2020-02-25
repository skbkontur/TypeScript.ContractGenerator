using System.Collections.Generic;
using System.Linq;

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
                
                if (typeInfo.Type.ToString() == "SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo")
                {
                    var foundType = GetSingleTypeName(memberAccess, node.ArgumentList).ToString();
                   
                    return ObjectCreationExpression(foundType);

                    Types.Add(RoslynTypeInfo.From(typeInfo.Type));
                }
            }

            return base.VisitInvocationExpression(node);
        }

        private static SyntaxNode ObjectCreationExpression(string foundType)
        {
            var identifier = SyntaxFactory.IdentifierName(typeInfoClassName);

            var argument = SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(foundType)));
            var argumentList = SyntaxFactory.SeparatedList(new[] {argument});

            var objectCreationExpression = SyntaxFactory.ObjectCreationExpression(identifier, SyntaxFactory.ArgumentList(argumentList), null);

            return objectCreationExpression;
        }

        private static TypeSyntax GetSingleGenericTypeNameFromGenericNameSyntax(SyntaxNode genericNameSyntax)
        {
            return genericNameSyntax.ChildNodes().OfType<TypeArgumentListSyntax>().Single().Arguments.Single();
        }

        private static TypeSyntax GetSingleTypeName(MemberAccessExpressionSyntax memberAccessExpressionSyntax, BaseArgumentListSyntax argumentListSyntax)
        {
            if (memberAccessExpressionSyntax.Name is GenericNameSyntax genericNameSyntax)
            {
                return GetSingleGenericTypeNameFromGenericNameSyntax(genericNameSyntax);
            }

            return GetSingleTypeNameFromArgumentListSyntax(argumentListSyntax);
        }

        private static TypeSyntax GetSingleTypeNameFromArgumentListSyntax(BaseArgumentListSyntax argumentListSyntax)
        {
            return argumentListSyntax.Arguments.Single().ChildNodes().OfType<TypeOfExpressionSyntax>().Single().ChildNodes().OfType<TypeSyntax>().Single();
        }

        private readonly SemanticModel semanticModel;

        private static List<ITypeInfo> Types = new List<ITypeInfo>();
        private const string typeInfoClassName ="MyClass";
    }
}