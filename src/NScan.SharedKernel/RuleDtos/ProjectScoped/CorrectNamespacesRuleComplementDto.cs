using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public sealed record CorrectNamespacesRuleComplementDto(
    Pattern ProjectAssemblyNamePattern)
  {
    public string RuleName { get; } = HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces;
  }
}
