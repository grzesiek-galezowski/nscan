using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingRules
{
  public class RuleUnionDtoAssertion : IRuleDtoVisitor
  {
    public virtual void Visit(HasAttributesOnRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<HasAttributesOnRuleComplementDto>()(dto);
    }

    public virtual void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<NoCircularUsingsRuleComplementDto>()(dto);
    }

    public virtual void Visit(NoUsingsRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<NoUsingsRuleComplementDto>()(dto);
    }

    public virtual void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<CorrectNamespacesRuleComplementDto>()(dto);
    }

    public virtual void Visit(IndependentRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<IndependentRuleComplementDto>()(dto);
    }

    public virtual void Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<HasTargetFrameworkRuleComplementDto>()(dto);
    }

    public virtual void Visit(HasPropertyRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<HasPropertyRuleComplementDto>()(dto);
    }
  }
}
