using System;

namespace SkbKontur.TypeScript.ContractGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ContractGeneratorGenerateAttribute : Attribute
    {
        public ContractGeneratorGenerateAttribute()
            : this(Scope.Self)
        {
        }

        public ContractGeneratorGenerateAttribute(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }

    [Flags]
    public enum Scope
    {
        Self = 1,
        Children = 2,
        NonAbstract = 4,
    }
}