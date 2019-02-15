using System;
using System.Linq;
using System.Reflection;

using CommandLine;

using SkbKontur.TypeScript.ContractGenerator.Cli.Utils;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    var (targetAssembly, targetAssemblyError) = AssemblyUtils
                        .GetAssemblies(o.Assembly)
                        .StartCollectionValidator()
                        .WithNoItemsError($"Assembly with name {o.Assembly} not found")
                        .WithManyItemsError(items => $"Found more than one assembly file matching name {o.Assembly}: [{string.Join(", ", items.Select(a => a.GetName()))}]")
                        .Single();

                    if (targetAssemblyError != null)
                    {
                        WriteError(targetAssemblyError);
                        return;
                    }

                    var (customTypeGenerator, customTypeGeneratorError) = targetAssembly
                        .GetImplementations<ICustomTypeGenerator>()
                        .StartCollectionValidator()
                        .WithNoItemsError($"Implementations of `ICustomTypeGenerator` not found in assembly {targetAssembly.GetName()}")
                        .WithManyItemsError($"Found more than one implementation of `ICustomTypeGenerator` in assembly {targetAssembly.GetName()}")
                        .Single();

                    if (customTypeGeneratorError != null)
                    {
                        WriteError(customTypeGeneratorError);
                        return;
                    }

                    var (rootTypesProvider, rootTypesProviderError) = targetAssembly
                        .GetImplementations<IRootTypesProvider>()
                        .StartCollectionValidator()
                        .WithNoItemsError($"Implementations of `IRootTypesProvider` not found in assembly {targetAssembly.GetName()}")
                        .WithManyItemsError($"Found more than one implementation of `IRootTypesProvider` in assembly {targetAssembly.GetName()}")
                        .Single();

                    if (rootTypesProviderError != null)
                    {
                        WriteError(rootTypesProviderError);
                        return;
                    }

                    var options = new FlowTypeGenerationOptions
                        {
                            EnumGenerationMode = o.EnumGenerationMode,
                            EnableExplicitNullability = o.EnableExplicitNullability,
                            EnableOptionalProperties = o.EnableOptionalProperties,
                        };

                    var flowTypeGenerator = new FlowTypeGenerator(options, customTypeGenerator, rootTypesProvider);

                    if (o.Language == Language.JavaScript)
                    {
                        flowTypeGenerator.GenerateFiles(o.OutputDirectory);
                    }
                    else
                    {
                        flowTypeGenerator.GenerateTypeScriptFiles(o.OutputDirectory);
                    }
                });
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var foundAssemblies = AssemblyUtils.GetAssemblies(args.Name.Split(',')[0]);
            return foundAssemblies.Length != 1 ? null : foundAssemblies[0];
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            WriteError($"Unexpected error occured: \n {exception?.Message ?? "no additional info was provided"}");
        }

        private static void WriteError(string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}