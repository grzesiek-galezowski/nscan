namespace NScan.Lib;

public static class FormattingExtensions
{
  extension(int i)
  {
    public string Spaces()
    {
      var str = "";
      for (int j = 0; j < i; j++)
      {
        str += " ";
      }

      return str;
    }

    public string Indentations()
    {
      return ((i + 1) * 2).Spaces();
    }
  }
}
