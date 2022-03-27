using NScan.Lib;

namespace NScan.SharedKernel;

public sealed record AssemblyName(string Value)
{
  public override string ToString()
  {
    return Value;
  }

  public bool Matches(Pattern pattern)
  {
    return pattern.IsMatchedBy(Value);
  }
}