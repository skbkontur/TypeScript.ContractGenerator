namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypeScriptUnitFactory
    {
        TypeScriptUnit GetOrCreateTypeUnit(string path);
    }
}