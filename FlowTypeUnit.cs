using System;
using System.Collections.Generic;
using System.Text;

using SKBKontur.Catalogue.FlowType.CodeDom;
using SKBKontur.Catalogue.FlowType.ContractGenerator.Internals;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator
{
    public class FlowTypeUnit
    {
        public string Path { get; set; }
        public IEnumerable<FlowTypeImportStatement> Imports { get { return imports.Values; } }
        public List<FlowTypeStatement> Body { get { return body; } }

        public FlowTypeTypeReference AddTypeImport(Type sourceType, FlowTypeTypeDeclaration typeDeclaration, FlowTypeUnit sourceUnit)
        {
            if (sourceUnit != this && !imports.ContainsKey(sourceType))
            {
                imports.Add(sourceType, new FlowTypeImportFromUnitStatement
                    {
                        TypeName = typeDeclaration.Name,
                        CurrentUnit = this,
                        TargetUnit = sourceUnit,
                    });
            }
            return new FlowTypeTypeReference(typeDeclaration.Name);
        }

        public FlowTypeVariableReference AddDefaultSymbolImport(string localName, string path)
        {
            var importedSymbol = new ImportedSymbol("default", localName, path);
            if (!symbolImports.ContainsKey(importedSymbol))
            {
                symbolImports.Add(importedSymbol, new FlowTypeImportDefaultFromPathStatement
                    {
                        TypeName = localName,
                        CurrentUnit = this,
                        PathToUnit = path,
                    });
            }
            return new FlowTypeVariableReference(localName);
        }

        public string GenerateCode(DefaultCodeGenerationContext context)
        {
            var result = new StringBuilder();

            foreach (var import in Imports)
            {
                result.Append(import.GenerateCode(context)).Append(context.NewLine);
            }
            foreach (var import in symbolImports.Values)
            {
                result.Append(import.GenerateCode(context)).Append(context.NewLine);
            }
            result.Append(context.NewLine);

            foreach (var statement in body)
            {
                result.Append(statement.GenerateCode(context)).Append(context.NewLine);
            }

            return result.ToString();
        }

        private readonly Dictionary<Type, FlowTypeImportStatement> imports = new Dictionary<Type, FlowTypeImportStatement>();
        private readonly Dictionary<ImportedSymbol, FlowTypeImportStatement> symbolImports = new Dictionary<ImportedSymbol, FlowTypeImportStatement>();

        private readonly List<FlowTypeStatement> body = new List<FlowTypeStatement>();
    }
}