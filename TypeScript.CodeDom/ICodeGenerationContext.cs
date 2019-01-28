namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public interface ICodeGenerationContext
    {
        JavaScriptTypeChecker TypeChecker { get; }
        string Tab { get; }
        string NewLine { get; }
        string GetReferenceFromUnitToAnother(string currentUnit, string targetUnit);
    }
}