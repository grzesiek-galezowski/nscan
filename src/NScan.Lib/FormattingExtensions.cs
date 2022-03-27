namespace NScan.Lib;

public static class FormattingExtensions
{
  public static string Spaces(this int i)
  {
    var str = "";
    for (int j = 0; j < i; j++)
    {
      str += " ";
    }

    return str;
  }

  public static string Indentations(this int i)
  {
    return ((i + 1) * 2).Spaces();
  }
}