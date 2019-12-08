using NScan.Lib.Union3;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
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