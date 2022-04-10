using System;
using NScan.Lib;

namespace NScan.NamespaceBasedRules;

public sealed record NamespaceName : IComparable<NamespaceName>
{
  private readonly string _value;

  public NamespaceName(string value)
  {
    _value = value;
  }

  public bool Matches(Pattern fromPattern)
  {
    return fromPattern.IsMatchedBy(_value);
  }

  public override string ToString()
  {
    return _value;
  }

  public int CompareTo(NamespaceName? other)
  {
    return string.Compare(_value, other?._value, StringComparison.Ordinal);
  }
}
