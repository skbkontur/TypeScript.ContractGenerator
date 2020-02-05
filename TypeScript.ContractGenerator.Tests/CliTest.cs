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
            BuildProjectByPath($"{pathToSlnDirectory}/AspNetCoreExample.Generator/AspNetCoreExample.Generator.csproj")
                .Should().BeTrue();
            BuildProjectByPath($"{pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/TypeScript.ContractGenerator.Cli.csproj")
                .Should().BeTrue();
                
            var cmdletForRunCli = $"dotnet {pathToCliDirectory}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                                  $"-a {pathToAspNetCoreExampleGeneratorAssemblyDirectory}/AspNetCoreExample.Generator.dll -o cliOutput";
            var generateProcess = RunCmdCommand(cmdletForRunCli);
            generateProcess.ExitCode.Should().Be(0);

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output/api";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, "cliOutput/api");
        }

        private static Process RunCmdCommand(string command)
        {
            var process = Process.Start("cmd.exe", "/C " + command);
            process?.WaitForExit();
            return process;
        }

        private static bool BuildProjectByPath(string pathToCsproj)
        {
            var cmdletForBuildAspNetCoreExampleGenerator = $"dotnet build {pathToCsproj}";
            var buildProcess = RunCmdCommand(cmdletForBuildAspNetCoreExampleGenerator);
            return buildProcess.ExitCode == 0;
        }
    }
}