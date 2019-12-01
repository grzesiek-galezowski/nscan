using NScan.Lib;
using NScan.Lib.Union1;
using NScan.Lib.Union2;
using NScan.Lib.Union3;
using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public interface IRuleDtoVisitor : IUnionVisitor<
    IndependentRuleComplementDto,
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto
  >,
    IUnionVisitor<IndependentRuleComplementDto>,
    IUnionVisitor<NoCircularUsingsRuleComplementDto>,
    IUnionVisitor<
      CorrectNamespacesRuleComplementDto,
      HasAttributesOnRuleComplementDto,
      HasTargetFrameworkRuleComplementDto
    >
  {
  }
  
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
}