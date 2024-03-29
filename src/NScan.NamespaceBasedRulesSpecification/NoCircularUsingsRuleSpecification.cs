﻿using LanguageExt;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScanSpecification.Lib;

namespace NScan.NamespaceBasedRulesSpecification;

public class NoCircularUsingsRuleSpecification
{
  [Fact]
  public void ShouldReturnDescriptionBasedOnDto()
  {
    //GIVEN
    var dto = Any.Instance<NoCircularUsingsRuleComplementDto>();
    var rule = new NoCircularUsingsRule(dto, Any.Instance<INamespaceBasedRuleViolationFactory>());

    //WHEN
    var description = rule.Description();

    //THEN
    description.Should().Be(
      new RuleDescription($"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName}"));
  }

  [Fact]
  public void ShouldReportErrorWhenNamespacesCacheContainsAnyCycles()
  {
    //GIVEN
    var ruleViolationFactory = Substitute.For<INamespaceBasedRuleViolationFactory>();
    var rule = new NoCircularUsingsRule(Any.Instance<NoCircularUsingsRuleComplementDto>(), ruleViolationFactory);
    var cache = Substitute.For<INamespacesDependenciesCache>();
    var report = Substitute.For<IAnalysisReportInProgress>();
    var cycles = Any.Seq<NamespaceDependencyPath>();
    var violation = Any.Instance<RuleViolation>();
    var projectAssemblyName = Any.Instance<AssemblyName>();

    cache.RetrieveCycles().Returns(cycles);
    ruleViolationFactory.NoCyclesRuleViolation(
      rule.Description(), projectAssemblyName, cycles).Returns(violation);

    //WHEN
    rule.Evaluate(projectAssemblyName, cache, report);

    //THEN
    report.Received(1).Add(violation);
  }

  [Fact]
  public void ShouldNotReportErrorWhenNamespacesCacheContainsNoCycles()
  {
    //GIVEN
    var rule = new NoCircularUsingsRule(
      Any.Instance<NoCircularUsingsRuleComplementDto>(), 
      Any.Instance<INamespaceBasedRuleViolationFactory>());
    var cache = Substitute.For<INamespacesDependenciesCache>();
    var report = Substitute.For<IAnalysisReportInProgress>();

    cache.RetrieveCycles().Returns(Seq<NamespaceDependencyPath>.Empty);

    //WHEN
    rule.Evaluate(Any.Instance<AssemblyName>(), cache, report);

    //THEN
    report.ReceivedNothing();
  }
}
