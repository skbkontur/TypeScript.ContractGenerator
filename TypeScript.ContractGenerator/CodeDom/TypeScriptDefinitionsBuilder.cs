namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptDefinitionsBuilder
    {
        public TypeScriptFileBuilder AddFile(string relativeFilename)
        {
            return new TypeScriptFileBuilder();
        }
    }
}