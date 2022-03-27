using System;

namespace NScan.DependencyPathBasedRules;

public class InvalidRuleException : Exception
{
  public InvalidRuleException(string ruleType) : base("Invalid rule: " + ruleType)
  {
      
  }
}