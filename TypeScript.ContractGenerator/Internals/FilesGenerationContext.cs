using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class FilesGenerationContext
    {
        private FilesGenerationContext(string fileExtension, Func<string, string> headerGenerationFunc, JavaScriptTypeChecker javaScriptTypeChecker)
        {
            FileExtension = fileExtension;
            HeaderGenerationFunc = headerGenerationFunc;
            JavaScriptTypeChecker = javaScriptTypeChecker;
        }

        public static FilesGenerationContext Create(JavaScriptTypeChecker javaScriptTypeChecker, LinterDisableMode linterDisableMode)
        {
            switch (javaScriptTypeChecker)
            {
            case JavaScriptTypeChecker.Flow:
                return new FilesGenerationContext("js", marker => $"// @flow\n{marker}\n", javaScriptTypeChecker);
            case JavaScriptTypeChecker.TypeScript:
                var linterDisable = linterDisableMode == LinterDisableMode.TsLint ? "tslint:disable" : "eslint-disable";
                return new FilesGenerationContext("ts", marker => $"// {linterDisable}\n{marker}\n", javaScriptTypeChecker);
            default:
                throw new ArgumentOutOfRangeException(nameof(javaScriptTypeChecker), javaScriptTypeChecker, null);
            }
        }

        public string FileExtension { get; }
        public Func<string, string> HeaderGenerationFunc { get; }
        public JavaScriptTypeChecker JavaScriptTypeChecker { get; }
    }
}