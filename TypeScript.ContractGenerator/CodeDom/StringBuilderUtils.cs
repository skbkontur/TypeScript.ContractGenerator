using System;
using System.Linq;
using System.Text;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public static class StringBuilderUtils
    {
        public static StringBuilder AppendWithTab(this StringBuilder stringBuilder, string tab, string lines, string newLine)
        {
            return stringBuilder.Append(string.Join(newLine, lines.Split(new[] {newLine}, StringSplitOptions.None).Select(x => tab + x)));
        }
    }
}