namespace TddXt.NScan.Domain.SharedKernel
{
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
  }
}