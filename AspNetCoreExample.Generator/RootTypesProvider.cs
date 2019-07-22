using System;
using System.Linq;

using AspNetCoreExample.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator;

namespace AspNetCoreExample.Generator
{
    public class RootTypesProvider : IRootTypesProvider
    {
        public Type[] GetRootTypes()
        {
            return typeof(UsersController).Assembly.GetTypes()
                                          .Where(x => typeof(ControllerBase).IsAssignableFrom(x))
                                          .ToArray();
        }
    }
}