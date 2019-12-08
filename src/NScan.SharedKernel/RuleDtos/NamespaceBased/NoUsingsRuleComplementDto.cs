using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public class NoUsingsRuleComplementDto
  {
    public Pattern ProjectAssemblyNamePattern { get; }
    public Pattern FromPattern { get; }
    public Pattern ToPattern { get; }
    public string RuleName { get; } = HasNoUsingsRuleMetadata.HasNoUsings;

    public NoUsingsRuleComplementDto(Pattern projectPattern, Pattern fromPattern, Pattern toPattern)
    {
      ProjectAssemblyNamePattern = projectPattern;
      FromPattern = fromPattern;
      ToPattern = toPattern;
    }
  }
}