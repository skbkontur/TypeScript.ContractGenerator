using System;

using CommandLine;

using SkbKontur.TypeScript.ContractGenerator.Cli.Utils;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    var targetAssembly  = AssemblyUtils.GetAssemblies(o.Assembly);

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
                        .GetImplementations<ITypesProvider>()
                        .StartCollectionValidator()
                        .WithNoItemsError($"Implementations of `IRootTypesProvider` not found in assembly {targetAssembly.GetName()}")
                        .WithManyItemsError($"Found more than one implementation of `IRootTypesProvider` in assembly {targetAssembly.GetName()}")
                        .Single();

                    if (rootTypesProviderError != null)
                    {
                        WriteError(rootTypesProviderError);
                        return;
                    }

                    var options = new TypeScriptGenerationOptions
                        {
                            EnumGenerationMode = o.EnumGenerationMode,
                            EnableExplicitNullability = o.EnableExplicitNullability,
                            EnableOptionalProperties = o.EnableOptionalProperties,
                        };

                    var typeGenerator = new TypeScriptGenerator(options, customTypeGenerator, rootTypesProvider);
                    typeGenerator.GenerateFiles(o.OutputDirectory, o.Language);
                });
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception; // question: А мы хотим вообще все ошибки подменять на свои? 
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