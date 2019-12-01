using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel
{
  public static class HasCorrectNamespacesRuleMetadata
  {
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";

    public static string Format(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

  }
}