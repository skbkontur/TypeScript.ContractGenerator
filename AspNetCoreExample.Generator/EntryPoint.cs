using System;
using System.IO;

using SkbKontur.TypeScript.ContractGenerator;

namespace AspNetCoreExample.Generator
{
    public static class EntryPoint
    {
        public static void Main(params string[] args)
        {
            var targetPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, "../../../output");
            var options = new TypeScriptGenerationOptions
                {
                    EnableOptionalProperties = false,
                    LinterDisableMode = LinterDisableMode.TsLint,
                    UseGlobalNullable = true,
                    NullabilityMode = NullabilityMode.Optimistic,
                };

            var typeScriptCodeGenerator = new TypeScriptGenerator(options, new AspNetCoreExampleCustomGenerator(), new TypesProvider());
            typeScriptCodeGenerator.GenerateFiles(targetPath);
        }
    }
}