using NScan.Lib.Union3;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public class ProjectScopedRuleNameExtractionVisitor :
    IUnionTransformingVisitor<
      CorrectNamespacesRuleComplementDto,
      HasAttributesOnRuleComplementDto,
      HasTargetFrameworkRuleComplementDto,
      string>
  {
    public string Visit(HasTargetFrameworkRuleComplementDto dto)
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
}