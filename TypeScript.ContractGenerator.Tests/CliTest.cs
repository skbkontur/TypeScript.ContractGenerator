using System.Diagnostics;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class CliTest
    {
        [Test]
        public void CliGenerated()
        {
            RunDotnetCommand($"{pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/{targetFramework}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                             $"-a {pathToSlnDirectory}/AspNetCoreExample.Api/bin/{configuration}/{targetFramework}/AspNetCoreExample.Api.dll " +
                             $"-o {TestContext.CurrentContext.TestDirectory}/cliOutput " +
                             "--nullabilityMode NullableReference " +
                             "--lintMode TsLint");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/cliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        [Test]
        public void RoslynCliGenerated()
        {
            RunDotnetCommand($"{pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/{targetFramework}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                             $"-d {pathToSlnDirectory}/AspNetCoreExample.Api " +
                             $"-a {typeof(ControllerBase).Assembly.Location} " +
                             $"-o {TestContext.CurrentContext.TestDirectory}/roslynCliOutput " +
                             "--nullabilityMode NullableReference " +
                             "--lintMode TsLint");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/roslynCliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        private static void RunDotnetCommand(string command)
        {
            var process = new Process
                {
                    StartInfo = new ProcessStartInfo("dotnet", command)
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false
                        },
                };

            process.Start();
            process.WaitForExit();

            process.ExitCode.Should().Be(0);
            process.StandardOutput.ReadToEnd().Trim().Should().Be("Generating TypeScript");
            process.StandardError.ReadToEnd().Trim().Should().BeEmpty();
        }

        private static readonly string pathToSlnDirectory = $"{TestContext.CurrentContext.TestDirectory}/../../../../";

        private const string targetFramework = "net8.0";

#if RELEASE
        private const string configuration = "Release";
#elif DEBUG
        private const string configuration = "Debug";
#endif
    }
}