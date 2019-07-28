﻿using System;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class IndependentRuleComplementDtoAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<IndependentRuleComplementDto> _action;

    public IndependentRuleComplementDtoAssertion(Action<IndependentRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(IndependentRuleComplementDto dto)
    {
      _action(dto);
    }
  }
}