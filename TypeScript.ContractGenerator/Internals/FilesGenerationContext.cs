using System;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class FilesGenerationContext
    {
        protected FilesGenerationContext(string fileExtension, Func<string, string> headerGenerationFunc)
        {
            FileExtension = fileExtension;
            HeaderGenerationFunc = headerGenerationFunc;
        }

        public static FilesGenerationContext Create(LinterDisableMode linterDisableMode)
        {
            var linterDisable = linterDisableMode == LinterDisableMode.TsLint ? "tslint:disable" : "eslint-disable";
            return new FilesGenerationContext("ts", marker => $"// {linterDisable}\n{marker}\n");
        }

        public string FileExtension { get; }
        public Func<string, string> HeaderGenerationFunc { get; }
    }
}