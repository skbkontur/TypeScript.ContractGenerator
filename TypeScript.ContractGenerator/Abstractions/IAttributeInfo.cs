using System.Collections.Generic;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IAttributeInfo
    {
        ITypeInfo AttributeType { get; }
        Dictionary<string, object> AttributeData { get; }
    }
}