using System.Linq;

using AspNetCoreExample.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace AspNetCoreExample.Generator
{
    public class TypesProvider : ITypesProvider
    {
        public ITypeInfo[] GetRootTypes()
        {
            return typeof(UsersController).Assembly
                                          .GetTypes()
                                          .Where(x => typeof(ControllerBase).IsAssignableFrom(x))
                                          .Select(x => (ITypeInfo)new TypeWrapper(x))
                                          .ToArray();
        }

        public ITypeInfo[] GetAssemblyTypes(ITypeInfo type)
        {
            return typeof(UsersController).Assembly
                                          .GetTypes()
                                          .Select(x => (ITypeInfo)new TypeWrapper(x))
                                          .ToArray();
        }
    }
}