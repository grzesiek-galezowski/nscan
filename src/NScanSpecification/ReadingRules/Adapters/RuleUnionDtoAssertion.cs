using TddXt.NScan.Lib;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class RuleUnionDtoAssertion : IUnion5Visitor<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto, NoCircularUsingsRuleComplementDto, HasAttributesOnRuleComplementDto, HasTargetFrameworkRuleComplementDto>
  {
    public virtual void Visit(HasAttributesOnRuleComplementDto dto)
    {
      FailWhenLambda.FailWhen<HasAttributesOnRuleComplementDto>()(dto);
    }

    public virtual void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      FailWhenLambda.FailWhen<NoCircularUsingsRuleComplementDto>()(dto);
    }

    public virtual void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      FailWhenLambda.FailWhen<CorrectNamespacesRuleComplementDto>()(dto);
    }

    public virtual void Visit(IndependentRuleComplementDto dto)
    {
      FailWhenLambda.FailWhen<IndependentRuleComplementDto>()(dto);
    }

    public virtual void Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      FailWhenLambda.FailWhen<HasTargetFrameworkRuleComplementDto>()(dto);
    }
  }
}