namespace NScan.SharedKernel.SharedKernel
{
  public class RuleViolation
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
  }
}