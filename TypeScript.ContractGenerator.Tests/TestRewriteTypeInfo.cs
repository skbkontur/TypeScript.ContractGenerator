using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            var project = AdhocProject.FromDirectory("../../../../AspNetCoreExample.Generator");
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult();
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));

            var root = tree.GetRoot();

            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();

            var nodes =
                root.DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Where(n => n.ToString().StartsWith("TypeInfo.From"));

            var dictionary = new Dictionary<InvocationExpressionSyntax, StatementSyntax>();

            foreach (var node in nodes)
            {
                var children = node.ChildNodes().ToArray();

                var memberAccessExpressionSyntax = children.OfType<MemberAccessExpressionSyntax>().Single();

                if (memberAccessExpressionSyntax.Expression.ToString() != "TypeInfo")
                {
                    continue;
                }

                var argumentListSyntax = children.OfType<ArgumentListSyntax>().Single();

                var newStatement = GetSingleTypeName(memberAccessExpressionSyntax, argumentListSyntax).ToString();

                var newHubSyntax = SyntaxFactory.ParseStatement(newStatement);

                dictionary.Add(node, newHubSyntax);
            }
            // https://stackoverflow.com/questions/31315500/replacing-multiple-nodes-in-roslyn-syntax-tree
            // root = root.ReplaceNodes(dictionary.Keys, (s, d) => dictionary[s]);   <--  так делать не выходит, потомучто меняем узлы, написанно выше 

            //https://joshvarty.com/2014/08/15/learn-roslyn-now-part-5-csharpsyntaxrewriter/  <-- походу нужно делать так 
            root = root.ReplaceNodes(dictionary.Keys, (s, d) => dictionary[s]);
        }

        private static TypeSyntax GetSingleGenericTypeNameFromGenericNameSyntax(SyntaxNode genericNameSyntax)
        {
            return genericNameSyntax.ChildNodes().OfType<TypeArgumentListSyntax>().Single().Arguments.OfType<NameSyntax>().Single();
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
    }
}