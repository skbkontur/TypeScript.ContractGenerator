namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IAttributeProvider
    {
        bool IsNameDefined(string name);
        object[] GetCustomAttributes(bool inherit);
    }
}