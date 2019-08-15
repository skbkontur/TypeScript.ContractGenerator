using System;
using System.IO;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace AspNetCoreExample.Generator
{
    public static class EntryPoint
    {
        public static void Main(params string[] args)
        {
            const string modelsNamespace = "AspNetCoreExample.Api.Models";
            var customTypeGenerator = new CustomTypeGenerator()
                .WithTypeRedirect<Guid>("Guid", @"dataTypes\Guid")
                .WithTypeLocationRule(
                    x => typeof(ControllerBase).IsAssignableFrom(x),
                    x => $"api/{x.Name}".Replace("Controller", "Api")
                )
                .WithTypeLocationRule(
                    x => x.FullName.StartsWith(modelsNamespace),
                    x => "dto/" + x.FullName.Substring(modelsNamespace.Length + 1).Replace(".", "/")
                )
                .WithTypeBuildingContext(ApiControllerTypeBuildingContext.Accept, (unit, type) => new ApiControllerTypeBuildingContext(unit, type));

            var typeScriptCodeGenerator = new TypeScriptGenerator(
                new TypeScriptGenerationOptions
                    {
                        EnableExplicitNullability = true,
                        EnableOptionalProperties = false,
                        EnumGenerationMode = EnumGenerationMode.TypeScriptEnum,
                        UseGlobalNullable = true,
                        NullabilityMode = NullabilityMode.Optimistic,
                    },
                customTypeGenerator,
                new RootTypesProvider()
                );
            var targetPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, "../../../output");

            typeScriptCodeGenerator.GenerateFiles(targetPath, JavaScriptTypeChecker.TypeScript);
        }
    }
}