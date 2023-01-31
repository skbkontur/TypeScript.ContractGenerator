using System.IO;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal static class FilesGenerator
    {
        public static void GenerateFiles(
            string targetDir,
            DefaultTypeScriptGeneratorOutput output,
            LinterDisableMode linterDisableMode,
            string? customContentMarker = null
        )
        {
            DeleteFiles(targetDir, "*.ts");
            Directory.CreateDirectory(targetDir);
            foreach (var unit in output.Units)
            {
                var targetFileName = GetUnitTargetFileName(targetDir, unit);

                EnsureDirectoryExists(targetFileName);

                var linterDisable = linterDisableMode == LinterDisableMode.TsLint ? "// tslint:disable" : "/* eslint-disable */";
                File.WriteAllText(targetFileName, $"{linterDisable}\n{GetContentMarker(customContentMarker)}\n");
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

        private static void DeleteFiles(string targetDir, string searchPattern, string? customContentMarker = null)
        {
            if (!Directory.Exists(targetDir))
                return;

            foreach (var file in Directory.GetFiles(targetDir, searchPattern, SearchOption.AllDirectories))
            {
                if (File.ReadAllText(file).Contains(GetContentMarker(customContentMarker)))
                    File.Delete(file);
            }
        }

        private static string GetContentMarker(string? customContentMarker) =>
            customContentMarker == null
                ? defaultContentMarkerString
                : $"// {customContentMarker.Replace("//", "")}";

        private const string defaultContentMarkerString = "// TypeScriptContractGenerator's generated content";
    }
}