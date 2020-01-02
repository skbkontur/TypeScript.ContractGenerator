namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IAttributeProvider
    {
        object[] GetCustomAttributes(bool inherit);
    }
}