using System.IO;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal static class FilesGenerator
    {
        public static void GenerateFiles(string targetDir, DefaultTypeScriptGeneratorOutput output, FilesGenerationContext filesGenerationContext)
        {
            DeleteFiles(targetDir, $"*.{filesGenerationContext.FileExtension}");
            Directory.CreateDirectory(targetDir);
            foreach (var unit in output.Units)
            {
                var targetFileName = GetUnitTargetFileName(targetDir, unit, filesGenerationContext.FileExtension);

                EnsureDirectoryExists(targetFileName);

                File.WriteAllText(targetFileName, filesGenerationContext.HeaderGenerationFunc(generatedContentMarkerString));
                File.AppendAllText(targetFileName, unit.GenerateCode(new DefaultCodeGenerationContext()));
            }
        }

        private static string GetUnitTargetFileName(string targetDir, TypeScriptUnit unit, string fileExtension)
        {
            var targetFileName = Path.Combine(targetDir, $"{unit.Path}.{fileExtension}");
            return targetFileName;
        }

        private static void EnsureDirectoryExists(string targetFileName)
        {
            var targetDirectoryName = Path.GetDirectoryName(targetFileName);
            if (!string.IsNullOrEmpty(targetDirectoryName))
                Directory.CreateDirectory(targetDirectoryName);
        }

        private static void DeleteFiles(string targetDir, string searchPattern)
        {
            if (!Directory.Exists(targetDir))
                return;

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