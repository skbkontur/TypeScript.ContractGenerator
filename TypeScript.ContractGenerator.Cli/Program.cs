using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using CommandLine;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Cli
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Parser.Default.ParseArguments<Options>(args).WithParsed(Process);
        }

        private static void Process(Options options)
        {
            GenerateByOptions(options);

            if (!options.Watch)
                return;

            WatchDirectory(Path.GetDirectoryName(options.Assembly), Debounce((source, e) => GenerateByOptions(options), 1000));
        }

        private static FileSystemEventHandler Debounce(FileSystemEventHandler func, int milliseconds = 1000)
        {
            CancellationTokenSource cancelTokenSource = null;
            return (arg1, arg2) =>
                {
                    cancelTokenSource?.Cancel();
                    cancelTokenSource = new CancellationTokenSource();

                    Task.Delay(milliseconds, cancelTokenSource.Token)
                        .ContinueWith(t =>
                            {
                                if (t.IsCompletedSuccessfully)
                                    func(arg1, arg2);
                            }, TaskScheduler.Default);
                };
        }

        private static void WatchDirectory(string directory, FileSystemEventHandler handler)
        {
            using var watcher = new FileSystemWatcher
                {
                    Path = directory,
                    NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName,
                    Filter = "*"
                };

            watcher.Changed += handler;
            watcher.Created += handler;
            watcher.Deleted += handler;

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press 'q' to quit");
            while (Console.Read() != 'q') ;
        }

        private static void GenerateByOptions(Options o)
        {
            Console.WriteLine("Generating TypeScript");

            ExecuteAndUnload(o.Assembly, out var testAlcWeakRef, o);

            // это нужно для того чтобы сборка из AssemblyLoadContext была выгруженна
            // https://docs.microsoft.com/en-us/dotnet/standard/assembly/unloadability?view=netcore-3.1
            for (var i = 0; testAlcWeakRef.IsAlive && i < 10; i++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                // здесь может случиться deadlock
                // https://github.com/dotnet/runtime/issues/535
                GC.WaitForPendingFinalizers();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef, Options options)
        {
            var alc = new DirectoryAssemblyLoadContext(assemblyPath);
            var targetAssembly = alc.LoadFromAssemblyPath(assemblyPath);

            alcWeakRef = new WeakReference(alc, trackResurrection : true);

            var customTypeGenerator = GetSingleImplementation<ICustomTypeGenerator>(targetAssembly);
            var typesProvider = GetSingleImplementation<ITypesProvider>(targetAssembly);
            if (customTypeGenerator == null || typesProvider == null)
                return;

            var typeGenerator = new TypeScriptGenerator(options.ToTypeScriptGenerationOptions(), customTypeGenerator, typesProvider);
            typeGenerator.GenerateFiles(options.OutputDirectory, JavaScriptTypeChecker.TypeScript);

            alc.Unload();
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