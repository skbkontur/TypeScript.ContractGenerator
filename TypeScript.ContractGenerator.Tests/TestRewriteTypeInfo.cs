using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis;
// тут явно что-то не так. Как я понял Microsoft.CSharp ссылается на System.CodeDom, но я такого не нашел, оказалось что это nuget пакет (мне кажется что это странно)
using Microsoft.CSharp;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class TestRewriteTypeInfo
    {
        [Test]
        public void Rewrite()
        {
            GetGeneratedCode();
        }

        private static string GetGeneratedCode()
        {
            var project = AdhocProject.FromDirectory($"{TestContext.CurrentContext.TestDirectory}/../../../../AspNetCoreExample.Generator");
            var types = new[] {typeof(object), typeof(HashSet<>), typeof(Internals.TypeInfo)};
            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult().AddReferences(types.Select(x => MetadataReference.CreateFromFile(x.Assembly.Location)));
            var tree = compilation.SyntaxTrees.Single(x => x.FilePath.Contains("ApiControllerTypeBuildingContext.cs"));

            var root = tree.GetRoot();

            var result = new TypeInfoRewriter(compilation.GetSemanticModel(tree)).Visit(root);
            var str = result.ToFullString();
            return str;
        }

        private static string GetFakeGeneratedCode()
        {
            return File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/Files/FakeApiControllerTypeBuildingContext.txt").Replace("\r\n", "\n");
        }

        [Test]
        public void ApiControllerTypeBuildingContextRewrite()
        {
            var expectedCode = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/Files/ApiControllerTypeBuildingContext.txt").Replace("\r\n", "\n");

            var str = GetGeneratedCode();

            str.Diff(expectedCode).ShouldBeEmpty();
        }

        [Test]
        public void ApiControllerTypeBuildingContextBeRewritedBuild()
        {
            var assemblies = AppDomain.CurrentDomain
                                      .GetAssemblies()
                                      .Where(a => !a.IsDynamic)
                                      .Select(a => a.Location)
                                      .ToArray();

            var types = new[] {typeof(RoslynTypeInfo)};
            var assemblies2 = types.Select(x => x.Assembly.Location).ToArray();

            var codeProvider = new CSharpCodeProvider();
           
            //var codeProvider = CodeDomProvider.CreateProvider("CSharp");
            
            // wtf? https://stackoverflow.com/questions/21078298/c-sharp-codedom-errors

            // походу мне надо новый фраймворк??
            var compilerParameters = new CompilerParameters();
            //compilerParameters.ReferencedAssemblies.Add("System.dll"); // System, System.Net, etc namespaces
            //var ttt = "C:/Projects/mentorskaya/TypeScript.ContractGenerator/TypeScript.ContractGenerator.Tests/bin/Debug/net472/SkbKontur.TypeScript.ContractGenerator.Roslyn";
            // compilerParameters.ReferencedAssemblies.Add(ttt); // System, System.Net, etc namespaces
            //compilerParameters.ReferencedAssemblies.Add("System.Data.dll"); // System.Data namespace
            //compilerParameters.ReferencedAssemblies.Add("System.Data.SQLite.dll"); // System.Data.SqlLite namespace
            //compilerParameters.ReferencedAssemblies.Add("System.Xml.dll"); // System.Xml namespace
            //compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll"); // System.Windows.Forms namespace

            compilerParameters.ReferencedAssemblies.AddRange(assemblies);
            //compilerParameters.ReferencedAssemblies.AddRange(assemblies2);
            // compilerParameters.CompilerOptions = "/t:library";
            compilerParameters.GenerateInMemory = true;

            var code = GetFakeGeneratedCode();

            var compilerResults = codeProvider.CompileAssemblyFromSource(compilerParameters, code);
            // тут ошибка
            // error CS0006: Metadata file 'System.Data.SQLite.dll' could not be found
            compilerResults.Errors.Count.Should().Be(0);

            /*if (compilerResults.Errors.Count > 0)
                throw new Exception($"Error ({compilerResults.Errors[0].ErrorText}) evaluating: {code}");

            var builderEvaluator = compilerResults.CompiledAssembly.CreateInstance("Eval.BuilderEvaluator");
            if (builderEvaluator == null)
                throw new Exception("Unable to create message builder evaluator: buildEvaluator instance is null");

            var getMessageMethodInfo = builderEvaluator.GetType().GetMethod("GetMessage");
            if (getMessageMethodInfo == null)
                throw new Exception("Unable to create message builder evaluator: getMessageMethodInfo is null");*/
        }
    }
}