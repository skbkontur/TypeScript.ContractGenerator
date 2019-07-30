using System;

namespace SkbKontur.TypeScript.ContractGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ContractGeneratorIgnoreAttribute : Attribute
    {
    }
}