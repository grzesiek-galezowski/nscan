using NScan.Lib;
using NScan.SharedKernel.RuleDtos;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class RuleUnionDtoAssertion : IUnion5Visitor<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto, NoCircularUsingsRuleComplementDto, HasAttributesOnRuleComplementDto, HasTargetFrameworkRuleComplementDto>
  {
    public virtual void Visit(HasAttributesOnRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<HasAttributesOnRuleComplementDto>()(dto);
    }

    public virtual void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      AssertionLambdas.FailWhen<NoCircularUsingsRuleComplementDto>()(dto);
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
  }
}