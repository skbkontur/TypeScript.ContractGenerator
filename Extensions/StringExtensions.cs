using System.Globalization;
using System.Text;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerCamelCase(this string input)
        {
            if(string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
                return input;
            var stringBuilder = new StringBuilder();
            for(var startIndex = 0; startIndex < input.Length; ++startIndex)
            {
                var flag = startIndex + 1 < input.Length;
                if(startIndex == 0 || !flag || char.IsUpper(input[startIndex + 1]))
                {
                    var lower = char.ToLower(input[startIndex], CultureInfo.InvariantCulture);
                    stringBuilder.Append(lower);
                }
                else
                {
                    stringBuilder.Append(input.Substring(startIndex));
                    break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}