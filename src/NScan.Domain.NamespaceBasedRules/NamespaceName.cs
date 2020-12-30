using System.Collections.Generic;
using NScan.Lib;
using Value;

namespace NScan.NamespaceBasedRules
{
  public sealed class NamespaceName : ValueType<NamespaceName>
  {
    private readonly string _value;

    public NamespaceName(string value)
    {
      _value = value;
    }

    public bool Matches(Pattern fromPattern)
    {
      return fromPattern.IsMatch(_value);
    }

    public override string ToString()
    {
      return _value;
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _value;
    }
  }
}
