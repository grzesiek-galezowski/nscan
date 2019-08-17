using System.Collections.Generic;
using Value;

namespace NScan.SharedKernel
{
  public class RuleViolation : ValueType<RuleViolation>
  {
    public RuleViolation(string ruleDescription, string prefixPhrase, string violationDescription)
    {
      RuleDescription = ruleDescription;
      PrefixPhrase = prefixPhrase;
      ViolationDescription = violationDescription;
    }

    public string RuleDescription { get; }
    public string PrefixPhrase { get; }
    public string ViolationDescription { get; }
    
    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return RuleDescription;
      yield return PrefixPhrase;
      yield return ViolationDescription;
    }
  }
}