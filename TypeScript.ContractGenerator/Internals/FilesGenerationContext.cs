using System;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class FilesGenerationContext
    {
        private FilesGenerationContext()
        {
        }

        [NotNull]
        public static FilesGenerationContext Create(JavaScriptTypeChecker javaScriptTypeChecker)
        {
            switch (javaScriptTypeChecker)
            {
            case JavaScriptTypeChecker.Flow:
                return new FilesGenerationContext
                    {
                        FileExtension = "js",
                        HeaderGenerationFunc = marker => $"// @flow\n{marker}\n",
                        JavaScriptTypeChecker = javaScriptTypeChecker
                    };
            case JavaScriptTypeChecker.TypeScript:
                return new FilesGenerationContext
                    {
                        FileExtension = "ts",
                        HeaderGenerationFunc = marker => $"// tslint:disable\n{marker}\n",
                        JavaScriptTypeChecker = javaScriptTypeChecker,
                    };
            default:
                throw new ArgumentOutOfRangeException(nameof(javaScriptTypeChecker), javaScriptTypeChecker, null);
            }
        }

        [NotNull]
        public string FileExtension { get; private set; }

        [NotNull]
        public Func<string, string> HeaderGenerationFunc { get; private set; }

        public JavaScriptTypeChecker JavaScriptTypeChecker { get; private set; }
    }
}