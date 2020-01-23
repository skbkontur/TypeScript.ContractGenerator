using System;
using System.Diagnostics;
using System.IO;

using FluentAssertions;

using NUnit.Framework;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class CliTest // понятно что тесты написанны откравенно плохо, но это пока для примера
    {
        static CliTest()
        {
            pathToSlnDirectory = AppDomain.CurrentDomain.BaseDirectory;
            for (var i = 0; i < 5; i++)
                pathToSlnDirectory = Directory.GetParent(pathToSlnDirectory).FullName;
            pathToAspNetCoreExampleGeneratorAssemblyDirectory = $"{pathToSlnDirectory}\\AspNetCoreExample.Generator\\bin\\Debug\\netcoreapp3.1";
            pathToCliDirectory = $"{pathToSlnDirectory}\\TypeScript.ContractGenerator.Cli\\bin\\Debug\\netcoreapp3.1";
        }

        [Test]
        public void DotnetToolExist()
        {
            const string command = "dotnet --version"; // думаю если расчитывать на netcore 3.1, то стоит версию dotnet какую-то определенную проверять > 3 например
            var process = RunCmdCommand(command);
            process.WaitForExit();
            process.ExitCode.Should().Be(0);
        }

        private static Process RunCmdCommand(string command)
        {
            var process = Process.Start("cmd.exe", "/C " + command);
            process?.WaitForExit();
            return process;
        }  

        [Test]
        public void CliGenerated()
        {
            AspNetCoreExampleGeneratorBuild();

            var cmdletForRunCli = $"dotnet {pathToCliDirectory}\\SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                                  $"-a {pathToAspNetCoreExampleGeneratorAssemblyDirectory}\\AspNetCoreExample.Generator.dll -o cliOutput";
            var generateProcess = RunCmdCommand(cmdletForRunCli);
            generateProcess.ExitCode.Should().Be(0);
            
            var expectedDirectory = $"{pathToSlnDirectory}\\AspNetCoreExample.Generator\\output\\api";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, "cliOutput\\api");
        }

        private static bool AspNetCoreExampleGeneratorBuild()
        {
            if (AspNetCoreExampleGeneratorAssemblyDirectoryExist()) return true;
            var cmdletForBuildAspNetCoreExampleGenerator = $"dotnet build {pathToSlnDirectory}\\AspNetCoreExample.Generator\\AspNetCoreExample.Generator.csproj";
            var buildProcess = RunCmdCommand(cmdletForBuildAspNetCoreExampleGenerator);
            return buildProcess.ExitCode == 0;
        }
        
        private static bool AspNetCoreExampleGeneratorAssemblyDirectoryExist()
        {
            return Directory.Exists(pathToAspNetCoreExampleGeneratorAssemblyDirectory);
        }

        private static readonly string pathToSlnDirectory;
        private static readonly string pathToAspNetCoreExampleGeneratorAssemblyDirectory;
        private static readonly string pathToCliDirectory;
    }
}