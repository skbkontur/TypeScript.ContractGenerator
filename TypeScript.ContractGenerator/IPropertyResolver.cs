using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IPropertyResolver
    {
        bool TryResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, Type type, PropertyInfo property, out TypeScriptTypeMemberDeclaration result);
    }
}