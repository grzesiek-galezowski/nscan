using System;

namespace NScan.Domain.Root
{
  public class InvalidRuleException : Exception
  {
    public InvalidRuleException(string ruleType) : base("Invalid rule: " + ruleType)
    {
      
    }
  }
}