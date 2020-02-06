using System.Diagnostics;

using FluentAssertions;

using NUnit.Framework;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class CliTest
    {
        private static readonly string pathToSlnDirectory = $"{TestContext.CurrentContext.TestDirectory}/../../../../";
        private static readonly string pathToAspNetCoreExampleGeneratorAssemblyDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/bin/Debug/netcoreapp3.1";
        private static readonly string pathToCliDirectory = $"{pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/Debug/netcoreapp3.1";

        [Test]
        public void CliGenerated()
        {
            BuildProjectByPath($"{pathToSlnDirectory}/AspNetCoreExample.Generator/AspNetCoreExample.Generator.csproj");
            BuildProjectByPath($"{pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/TypeScript.ContractGenerator.Cli.csproj");

            RunCmdCommand($"dotnet {pathToCliDirectory}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                          $"-a {pathToAspNetCoreExampleGeneratorAssemblyDirectory}/AspNetCoreExample.Generator.dll " +
                          $"-o {TestContext.CurrentContext.TestDirectory}/cliOutput " +
                          "--nullabilityMode Optimistic " +
                          "--lintMode TsLint " +
                          "--globalNullable true");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/cliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        private static void RunCmdCommand(string command)
        {
            var process = new Process
                {
                    StartInfo =
                        {
                            FileName = "cmd.exe",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = "/C " + command,
                        }
                };
            process.Start();
            process.WaitForExit();
            process.ExitCode.Should().Be(0);
        }

        private static void BuildProjectByPath(string pathToCsproj)
        {
            RunCmdCommand($"dotnet build {pathToCsproj}");
        }
    }
}