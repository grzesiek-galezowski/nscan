using NScan.Lib.Union1;
using NScan.Lib.Union3;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
 
  public interface IPathBasedRuleDtoVisitor : IUnionVisitor<IndependentRuleComplementDto>
  {
  }

  public interface INamespaceBasedRuleDtoVisitor : IUnionVisitor<NoCircularUsingsRuleComplementDto>
  {
  }

  public interface IProjectScopedRuleDtoVisitor : 
    IUnionVisitor<
      CorrectNamespacesRuleComplementDto,
      HasAttributesOnRuleComplementDto,
      HasTargetFrameworkRuleComplementDto
    >
  {
  }

  public interface IRuleDtoVisitor :
    IPathBasedRuleDtoVisitor,
    INamespaceBasedRuleDtoVisitor,
    IProjectScopedRuleDtoVisitor
  {
  }
}