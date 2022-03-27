using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased;

public sealed record NoUsingsRuleComplementDto(
  Pattern ProjectAssemblyNamePattern, 
  Pattern FromPattern, 
  Pattern ToPattern)
{
  public string RuleName => HasNoUsingsRuleMetadata.HasNoUsings;
}