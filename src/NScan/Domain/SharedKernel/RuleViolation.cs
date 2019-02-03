namespace TddXt.NScan.Domain.SharedKernel
{
  public class RuleViolation
  {
    public RuleViolation(string ruleDescription, string prefixPhrase, string violationDescription)
    {
      RuleDescription = ruleDescription;
      PrefixPhrase = prefixPhrase;
      ViolationDescription = violationDescription;
    }

    public string RuleDescription { get; private set; }
    public string PrefixPhrase { get; private set; }
    public string ViolationDescription { get; private set; }
  }
}