using System;

namespace NScan.SharedKernel.SharedKernel
{
  public class InvalidRuleException : Exception
  {
    public InvalidRuleException(string ruleType) : base("Invalid rule: " + ruleType)
    {
      
    }
  }
}