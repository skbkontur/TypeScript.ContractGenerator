using System.IO;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal static class FilesGenerator
    {
        public const string JavaScriptFilesExtension = "js";
        public const string TypeScriptFilesExtension = "ts";

        public static void GenerateFiles(string targetDir, DefaultFlowTypeGeneratorOutput output)
        {
            Directory.CreateDirectory(targetDir);
            var files = Directory.GetFiles(targetDir, $"*.{JavaScriptFilesExtension}", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (File.ReadAllText(file).Contains(generatedContentMarkerString))
                {
                    File.Delete(file);
                }
            }
            foreach (var unit in output.Units)
            {
                var targetFileName = GetUnitTargetFileName(targetDir, unit, JavaScriptFilesExtension);

                EnsureDirectoryExists(targetFileName);

                File.WriteAllText(targetFileName, "// @flow" + "\n");
                File.AppendAllText(targetFileName, generatedContentMarkerString + "\n");
                File.AppendAllText(targetFileName, unit.GenerateCode(new DefaultCodeGenerationContext(JavaScriptTypeChecker.Flow)));
            }
        }

        public static void GenerateTypeScriptFiles(string targetDir, DefaultFlowTypeGeneratorOutput output)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var unit in output.Units)
            {
                var targetFileName = GetUnitTargetFileName(targetDir, unit, TypeScriptFilesExtension);

                EnsureDirectoryExists(targetFileName);

                File.AppendAllText(targetFileName, generatedContentMarkerString + "\n");
                File.AppendAllText(targetFileName, "// tslint:disable" + "\n");
                File.AppendAllText(targetFileName, unit.GenerateCode(new DefaultCodeGenerationContext(JavaScriptTypeChecker.TypeScript)));
            }
        }

        private static string GetUnitTargetFileName(string targetDir, FlowTypeUnit unit, string fileExtension)
        {
            var targetFileName = Path.Combine(targetDir, unit.Path + $".{fileExtension}");
            return targetFileName;
        }

        private static void EnsureDirectoryExists(string targetFileName)
        {
            var targetDirectoryName = Path.GetDirectoryName(targetFileName);
            if (!string.IsNullOrEmpty(targetDirectoryName))
                Directory.CreateDirectory(targetDirectoryName);
        }

        public static void DeleteFiles(string targetDir, string searchPattern)
        {
            var files = Directory.GetFiles(targetDir, searchPattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (File.ReadAllText(file).Contains(generatedContentMarkerString))
                {
                    File.Delete(file);
                }
            }
        }

        private static readonly string generatedContentMarkerString = "// TypeScriptContractGenerator's generated content";
    }
}