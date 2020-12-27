using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public record NamespaceName(string Value)
  {
    public bool Matches(Pattern fromPattern)
    {
      return fromPattern.IsMatch(Value);
    }
  }
}
