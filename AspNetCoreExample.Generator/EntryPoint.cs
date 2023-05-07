using AspNetCoreExample.Api.TypeScriptConfiguration;

using SkbKontur.TypeScript.ContractGenerator;

namespace AspNetCoreExample.Generator;

public static class EntryPoint
{
    public static void Main(params string[] args)
    {
        var targetPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, "../../../output");
        var options = new TypeScriptGenerationOptions
            {
                NullabilityMode = NullabilityMode.NullableReference,
                LinterDisableMode = LinterDisableMode.TsLint
            };
        var typeScriptCodeGenerator = new TypeScriptGenerator(options, new CustomGenerator(), new TypesProvider());
        typeScriptCodeGenerator.GenerateFiles(targetPath);
    }
}