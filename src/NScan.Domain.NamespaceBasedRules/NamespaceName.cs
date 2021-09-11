using System;
using System.Collections.Generic;
using NScan.Lib;
using Value;

namespace NScan.NamespaceBasedRules
{
  public sealed class NamespaceName : ValueType<NamespaceName>, IComparable<NamespaceName>
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

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _value;
    }

    public int CompareTo(NamespaceName other)
    {
      return string.Compare(_value, other._value, StringComparison.Ordinal);
    }
  }
}
