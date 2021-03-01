﻿using FluentAssertions;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using TddXt.XFluentAssert.Api;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.Root
{
  public class NamespaceBasedRuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateNoCircularDependenciesRule()
    {
      //GIVEN
      var violationFactory = Any.Instance<INamespaceBasedRuleViolationFactory>();
      var ruleFactory = new NamespaceBasedRuleFactory(violationFactory);
      var ruleDto = Any.Instance<NoCircularUsingsRuleComplementDto>();

      //WHEN
      var rule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      rule.Should().BeOfType<NoCircularUsingsRule>();
      rule.Should().DependOn(ruleDto);
      rule.Should().DependOn(violationFactory);
    }
    
    [Fact]
    public void ShouldCreateNoUsingsRule()
    {
      //GIVEN
      var violationFactory = Any.Instance<INamespaceBasedRuleViolationFactory>();
      var ruleFactory = new NamespaceBasedRuleFactory(violationFactory);
      var ruleDto = Any.Instance<NoUsingsRuleComplementDto>();

      //WHEN
      var rule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      rule.Should().BeOfType<NoUsingsRule>();
      rule.Should().DependOn(ruleDto);
      rule.Should().DependOn(violationFactory);
    }
  }
}
