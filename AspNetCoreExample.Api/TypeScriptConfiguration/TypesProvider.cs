using AspNetCoreExample.Api.Controllers;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace AspNetCoreExample.Api.TypeScriptConfiguration;

public class TypesProvider : IRootTypesProvider
{
    public ITypeInfo[] GetRootTypes()
    {
        return new[]
            {
                TypeInfo.From<WeatherForecastController>(),
                TypeInfo.From<UserController>(),
                TypeInfo.From<NotesController>()
            };
    }
}