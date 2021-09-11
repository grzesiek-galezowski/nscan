using System;
using System.Collections.Generic;
using Value;

namespace NScan.SharedKernel
{
  public class RuleViolation : ValueType<RuleViolation>
  {
    public RuleViolation(RuleDescription description, string prefixPhrase, string violationDescription)
    {
      RuleDescription = description;
      PrefixPhrase = prefixPhrase;
      ViolationDescription = violationDescription;
    }

    public static RuleViolation Create(string ruleDescription, string prefixPhrase, string violationDescription)
    {
      return new RuleViolation(new RuleDescription(ruleDescription), prefixPhrase, violationDescription);
    }

    public string PrefixPhrase { get; }
    public string ViolationDescription { get; }
    public RuleDescription RuleDescription { get; }
    
    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return RuleDescription;
      yield return PrefixPhrase;
      yield return ViolationDescription;
    }

  }

  //bug move elsewhere
  public record RuleDescription(string Value)
  {
    public override string ToString()
    {
      return Value;
    }
  }

}
