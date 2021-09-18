using System.Collections.Generic;
using FluentAssertions;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.NamespaceBasedRules
{
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
      var cycles = Any.ReadOnlyList<NamespaceDependencyPath>();
      var violation = Any.Instance<RuleViolation>();
      var projectAssemblyName = Any.String();

      cache.RetrieveCycles().Returns(cycles);
      ruleViolationFactory.NoCyclesRuleViolation(
        rule.Description(), 
        projectAssemblyName, 
        cycles).Returns(violation);

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

      cache.RetrieveCycles().Returns(EmptyList());

      //WHEN
      rule.Evaluate(Any.String(), cache, report);

      //THEN
      report.ReceivedNothing();
    }

    private static List<NamespaceDependencyPath> EmptyList()
    {
      return new List<NamespaceDependencyPath>();
    }
  }
}
