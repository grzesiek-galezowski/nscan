using System.Text.RegularExpressions;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

static internal class TypeFormatting
{
  public static string StripWhitespace(string text)
  {
    return Regex.Replace(text, "[\\s\\t]+", string.Empty);
  }
}