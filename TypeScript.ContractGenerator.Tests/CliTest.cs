using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

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
            RunCmdCommand($"dotnet {pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/{targetFramework}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                          $"-a {pathToSlnDirectory}/AspNetCoreExample.Api/bin/{configuration}/net7.0/AspNetCoreExample.Api.dll " +
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
            RunCmdCommand($"dotnet {pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/{targetFramework}/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                          $"-d {pathToSlnDirectory}/AspNetCoreExample.Api " +
                          $"-a {typeof(ControllerBase).Assembly.Location} " +
                          $"-o {TestContext.CurrentContext.TestDirectory}/roslynCliOutput " +
                          "--nullabilityMode NullableReference " +
                          "--lintMode TsLint");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/roslynCliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        private static void RunCmdCommand(string command)
        {
            ProcessStartInfo processStartInfo;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var commandParts = command.Split(new[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
                processStartInfo = new ProcessStartInfo(commandParts.First(), commandParts.Last());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                processStartInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                    {
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
            }
            else
            {
                throw new NotSupportedException($"The current platform {RuntimeInformation.OSDescription} is not supported.");
            }

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            var testProcess = new Process
                {
                    StartInfo = processStartInfo
                };

            testProcess.Start();
            testProcess.WaitForExit();
            TestContext.Out.WriteLine(testProcess.StandardOutput.ReadToEnd());
            TestContext.Error.WriteLine(testProcess.StandardError.ReadToEnd());
            testProcess.ExitCode.Should().Be(0);
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