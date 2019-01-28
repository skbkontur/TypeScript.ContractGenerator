namespace TypeScript.CodeDom
{
    public class FlowTypeDefinitionsBuilder
    {
        public FlowTypeFileBuilder AddFile(string relativeFilename)
        {
            return new FlowTypeFileBuilder();
        }
    }
}