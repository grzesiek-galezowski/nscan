using NScan.SharedKernel.RuleDtos;

namespace NScan.SharedKernel
{
  public static class HasNoCircularUsingsRuleMetadata
  {
    public const string HasNoCircularUsings = "hasNoCircularUsings";

    public static string Format(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }
  }
}