using System.IO;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.Internals
{
    internal static class FilesGenerator
    {
        public static void GenerateFiles(string targetDir, DefaultFlowTypeGeneratorOutput output)
        {
            Directory.CreateDirectory(targetDir);
            var files = Directory.GetFiles(targetDir, "*.js", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (File.ReadAllText(file).Contains(generatedContentMarkerString))
                {
                    File.Delete(file);
                }
            }
            foreach (var unit in output.Units)
            {
                var targetFileName = Path.Combine(targetDir, unit.Path + ".js");
                Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
                File.WriteAllText(targetFileName, "// @flow" + "\n");
                File.AppendAllText(targetFileName, generatedContentMarkerString + "\n");
                File.AppendAllText(targetFileName, unit.GenerateCode(new DefaultCodeGenerationContext()));
            }
        }

        private static readonly string generatedContentMarkerString = "// FlowTypeContractGenerator's generated content";
    }
}