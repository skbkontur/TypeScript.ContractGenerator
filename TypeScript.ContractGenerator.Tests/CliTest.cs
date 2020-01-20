using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        public void AssemblyDirectoryExistTest()
        {
            DirectoryAssert.Exists($"{pathToAspNetCoreExampleGeneratorAssemblyDirectory}");
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
            return Process.Start("cmd.exe", "/C " + command);
        }  
        
        private static Process RunCmdCommand2(string command)
        {
            var process = new Process
                {
                    StartInfo =
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c {command}",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        }
                };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            var err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return process;
        }

        [Test]
        public void CliGenerated()
        {
            var cmdletForBuildAspNetCoreExampleGenerator = $"dotnet build {pathToSlnDirectory}\\AspNetCoreExample.Generator\\AspNetCoreExample.Generator.csproj";
            var buildProcess = RunCmdCommand(cmdletForBuildAspNetCoreExampleGenerator);
            buildProcess.WaitForExit();
            buildProcess.ExitCode.Should().Be(0);

            var cmdletForRunCli = $"dotnet {pathToCliDirectory}\\SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                                  $"-a {pathToAspNetCoreExampleGeneratorAssemblyDirectory}\\AspNetCoreExample.Generator.dll -o cliOutput";
            var generateProcess = RunCmdCommand(cmdletForRunCli);
            generateProcess.WaitForExit();
            
            generateProcess.ExitCode.Should().Be(0);

            var allGeneratedFiles = Directory.EnumerateFiles("cliOutput\\api", "*", SearchOption.TopDirectoryOnly).ToArray();
            var allExpectedFiles = Directory.EnumerateFiles($"{pathToSlnDirectory}\\AspNetCoreExample.Generator\\output\\api").ToArray();

            allExpectedFiles.Length.Should().Be(allGeneratedFiles.Length);

            for (var i = 0; i < allGeneratedFiles.Length; i++)
            {
                GetFileText(allGeneratedFiles[i]).Diff(GetFileText(allExpectedFiles[i])).ShouldBeEmpty();
            }
        }

        private static string GetFileText(string filePath)
        {
            return File.ReadAllText(filePath).Replace("\r\n", "\n");
        }

        private static readonly string pathToSlnDirectory;
        private static readonly string pathToAspNetCoreExampleGeneratorAssemblyDirectory;
        private static readonly string pathToCliDirectory;
    }
}