using System.Linq;

using AspNetCoreExample.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace AspNetCoreExample.Generator
{
    public class TypesProvider : IRootTypesProvider
    {
        public ITypeInfo[] GetRootTypes()
        {
            return TypeInfo.From<UsersController>().Assembly
                           .GetTypes()
                           .Where(x => TypeInfo.From<ControllerBase>().IsAssignableFrom(x))
                           .ToArray();
        }
    }
}