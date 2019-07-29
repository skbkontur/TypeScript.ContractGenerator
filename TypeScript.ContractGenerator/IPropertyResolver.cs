using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IPropertyResolver
    {
        TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, Type type, PropertyInfo property);
    }
}