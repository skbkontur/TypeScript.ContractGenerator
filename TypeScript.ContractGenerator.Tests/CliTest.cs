using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            var output = Path.Combine(TestContext.CurrentContext.TestDirectory, "cliOutput");

            RunDotnetCommand($"{Tool()} -a {Assembly()} -o {output} --nullabilityMode NullableReference --lintMode TsLint");

            var expectedDirectory = $"{repoRoot}/AspNetCoreExample.Generator/output";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, output, generatedOnly : true);
        }

        [Test]
        public void RoslynCliGenerated()
        {
            var project = Path.Combine(repoRoot, "AspNetCoreExample.Api");
            var assembly = typeof(ControllerBase).Assembly.Location;
            var output = Path.Combine(TestContext.CurrentContext.TestDirectory, "roslynCliOutput");

            RunDotnetCommand($"{Tool()} -d {project} -a {assembly} -o {output} --nullabilityMode NullableReference --lintMode TsLint");

            var expectedDirectory = $"{repoRoot}/AspNetCoreExample.Generator/output";
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

            process.StandardError.ReadToEnd().Trim().Should().BeEmpty();
            process.StandardOutput.ReadToEnd().Trim().Should().Be("Generating TypeScript");
            process.ExitCode.Should().Be(0);
        }

        private static string Tool()
        {
            return Path.Combine(
                repoRoot,
                "TypeScript.ContractGenerator.Cli",
                "bin",
                configuration,
                targetFramework,
                "SkbKontur.TypeScript.ContractGenerator.Cli.dll"
            );
        }

        private static string Assembly()
        {
            return Path.Combine(
                repoRoot,
                "AspNetCoreExample.Api",
                "bin",
                configuration,
                targetFramework,
                "AspNetCoreExample.Api.dll"
            );
        }

        private static string RootDirectory(DirectoryInfo? current = null)
        {
            current ??= new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (current != null && current.EnumerateFiles().All(x => x.Name != "TypeScript.ContractGenerator.sln"))
            {
                current = current.Parent;
            }

            return current?.FullName ?? throw new InvalidOperationException("Cannot find root folder");
        }

        private static readonly string repoRoot = RootDirectory();

        private const string targetFramework = "net8.0";

#if RELEASE
        private const string configuration = "Release";
#elif DEBUG
        private const string configuration = "Debug";
#endif
    }
}