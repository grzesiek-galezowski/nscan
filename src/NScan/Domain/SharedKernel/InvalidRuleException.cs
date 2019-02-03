using System;

namespace TddXt.NScan.Domain.SharedKernel
{
  public class InvalidRuleException : Exception
  {
    public InvalidRuleException(string ruleType) : base("Invalid rule: " + ruleType)
    {
      
    }
  }
}