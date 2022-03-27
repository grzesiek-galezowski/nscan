using NScan.Lib.Union4;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped;

public class ProjectScopedRuleNameExtractionVisitor :
  IUnionTransformingVisitor<
    CorrectNamespacesRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto,
    HasPropertyRuleComplementDto,
    string>
{
  public string Visit(HasTargetFrameworkRuleComplementDto dto)
  {
    return dto.RuleName;
  }

  public string Visit(HasPropertyRuleComplementDto dto)
  {
    return dto.RuleName;
  }

  public string Visit(HasAttributesOnRuleComplementDto dto)
  {
    return dto.RuleName;
  }

  public string Visit(CorrectNamespacesRuleComplementDto dto)
  {
    return dto.RuleName;
  }
}