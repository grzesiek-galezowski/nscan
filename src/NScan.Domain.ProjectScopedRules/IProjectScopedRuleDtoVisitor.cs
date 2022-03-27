using NScan.Lib.Union4;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules;

public interface IProjectScopedRuleDtoVisitor : 
  IUnionVisitor<
    CorrectNamespacesRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto,
    HasPropertyRuleComplementDto
  >
{
}