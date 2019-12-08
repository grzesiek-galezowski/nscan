using NScan.Lib.Union3;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel.RuleDtos
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