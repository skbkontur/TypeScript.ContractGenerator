using System.IO;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal static class FilesGenerator
    {
        public static void GenerateFiles(
            string targetDir,
            DefaultTypeScriptGeneratorOutput output,
            LinterDisableMode linterDisableMode,
            string? projectId = null
        )
        {
            DeleteFiles(targetDir, "*.ts");
            Directory.CreateDirectory(targetDir);
            foreach (var unit in output.Units)
            {
                var targetFileName = GetUnitTargetFileName(targetDir, unit);

                EnsureDirectoryExists(targetFileName);

                var linterDisable = linterDisableMode == LinterDisableMode.TsLint ? "// tslint:disable" : "/* eslint-disable */";
                File.WriteAllText(targetFileName, $"{linterDisable}\n{GetGeneratedContentMarkerString(projectId)}\n");
                File.AppendAllText(targetFileName, unit.GenerateCode(new DefaultCodeGenerationContext()));
            }
        }

        private static string GetUnitTargetFileName(string targetDir, TypeScriptUnit unit)
        {
            var targetFileName = Path.Combine(targetDir, $"{unit.Path}.ts");
            return targetFileName;
        }

        private static void EnsureDirectoryExists(string targetFileName)
        {
            var targetDirectoryName = Path.GetDirectoryName(targetFileName);
            if (!string.IsNullOrEmpty(targetDirectoryName))
                Directory.CreateDirectory(targetDirectoryName);
        }

        private static void DeleteFiles(string targetDir, string searchPattern, string? projectId = null)
        {
            if (!Directory.Exists(targetDir))
                return;

            foreach (var file in Directory.GetFiles(targetDir, searchPattern, SearchOption.AllDirectories))
            {
                if (File.ReadAllText(file).Contains(GetGeneratedContentMarkerString(projectId)))
                    File.Delete(file);
            }
        }

        private const string generatedContentMarkerStringPrefix = "// TypeScriptContractGenerator's generated content";

        private static string GetGeneratedContentMarkerString(string? projectId = null) =>
            projectId == null
                ? generatedContentMarkerStringPrefix
                : $"{generatedContentMarkerStringPrefix} for {projectId}";
    }
}