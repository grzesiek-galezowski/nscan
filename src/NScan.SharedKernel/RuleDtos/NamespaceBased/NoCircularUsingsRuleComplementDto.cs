using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased;

public sealed record NoCircularUsingsRuleComplementDto(Pattern ProjectAssemblyNamePattern)
{
  public string RuleName => HasNoCircularUsingsRuleMetadata.HasNoCircularUsings;
}