using System;

namespace TddXt.NScan.Domain
{
  public class InvalidRuleException : Exception
  {
    public InvalidRuleException(string ruleType) : base("Invalid rule: " + ruleType)
    {
      
    }
  }
}