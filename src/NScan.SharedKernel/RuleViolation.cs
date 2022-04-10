namespace NScan.SharedKernel;

public sealed record RuleViolation(
  RuleDescription RuleDescription, 
  string PrefixPhrase, 
  string ViolationDescription)
{
  public static RuleViolation Create(RuleDescription description, string prefixPhrase, string violationDescription)
  {
    return new RuleViolation(description, prefixPhrase, violationDescription);
  }

  public string ToHumanReadableString()
  {
    return PrefixPhrase + ViolationDescription;
  }
}
