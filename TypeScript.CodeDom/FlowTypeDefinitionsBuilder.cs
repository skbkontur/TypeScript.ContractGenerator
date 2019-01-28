namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeDefinitionsBuilder
    {
        public FlowTypeFileBuilder AddFile(string relativeFilename)
        {
            return new FlowTypeFileBuilder();
        }
    }
}