using System;
using System.Linq;
using System.Reflection;

using CommandLine;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    var targetAssembly = AssemblyUtils.GetAssemblies(o.Assembly);
                    var customTypeGenerator = GetSingleImplementation<ICustomTypeGenerator>(targetAssembly);
                    var typesProvider = GetSingleImplementation<ITypesProvider>(targetAssembly);
                    if (customTypeGenerator == null || typesProvider == null)
                        return;

                    var options = new TypeScriptGenerationOptions
                        {
                            EnableExplicitNullability = o.EnableExplicitNullability,
                            EnableOptionalProperties = o.EnableOptionalProperties,
                            EnumGenerationMode = o.EnumGenerationMode,
                            UseGlobalNullable = o.UseGlobalNullable,
                            NullabilityMode = o.NullabilityMode,
                            LinterDisableMode = o.LinterDisableMode,
                        };
                    var typeGenerator = new TypeScriptGenerator(options, customTypeGenerator, typesProvider);
                    typeGenerator.GenerateFiles(o.OutputDirectory, JavaScriptTypeChecker.TypeScript);
                });
        }

        private static T GetSingleImplementation<T>(Assembly assembly)
            where T : class
        {
            var implementations = assembly.GetImplementations<T>();
            if (!implementations.Any())
                WriteError($"Implementations of `{typeof(T).Name}` not found in assembly {assembly.GetName()}");
            if (implementations.Length != 1)
                WriteError($"Found more than one implementation of `{typeof(T).Name}` in assembly {assembly.GetName()}");
            return implementations.Length == 1 ? implementations[0] : null;
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