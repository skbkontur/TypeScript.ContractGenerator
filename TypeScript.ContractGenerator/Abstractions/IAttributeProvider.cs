namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface IAttributeProvider
    {
        IAttributeInfo[] GetAttributes(bool inherit);
    }
}