using NScan.Lib.Union3;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public interface IProjectScopedRuleDtoVisitor : 
    IUnionVisitor<
      CorrectNamespacesRuleComplementDto,
      HasAttributesOnRuleComplementDto,
      HasTargetFrameworkRuleComplementDto
    >
  {
  }
}