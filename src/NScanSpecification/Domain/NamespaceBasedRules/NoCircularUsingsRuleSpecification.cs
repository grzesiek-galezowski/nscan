using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NoCircularUsingsRuleSpecification
  {
    [Fact]
    public void ShouldReturnDescriptionBasedOnDto()
    {
      //GIVEN
      var dto = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();
      var rule = new NoCircularUsingsRule(dto, AnyRoot.Root.Any.Instance<IRuleViolationFactory>());

      //WHEN
      var description = rule.Description();

      //THEN
      description.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    [Fact]
    public void ShouldReportErrorWhenNamespacesCacheContainsAnyCycles()
    {
      //GIVEN
      var ruleViolationFactory = Substitute.For<IRuleViolationFactory>();
      var rule = new NoCircularUsingsRule(AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>(), ruleViolationFactory);
      var cache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var cycles = AnyRoot.Root.Any.ReadOnlyList<IReadOnlyList<string>>();
      var violation = AnyRoot.Root.Any.Instance<RuleViolation>();
      var projectAssemblyName = AnyRoot.Root.Any.String();

      cache.RetrieveCycles().Returns(cycles);
      ruleViolationFactory.NoCyclesRuleViolation(rule.Description(), projectAssemblyName, cycles).Returns(violation);

      //WHEN
      rule.Evaluate(projectAssemblyName, cache, report);

      //THEN
      report.Received(1).Add(violation);
    }

    [Fact]
    public void ShouldNotReportErrorWhenNamespacesCacheContainsNoCycles()
    {
      //GIVEN
      var rule = new NoCircularUsingsRule(AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>(), AnyRoot.Root.Any.Instance<INamespaceBasedRuleViolationFactory>());
      var cache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();

      cache.RetrieveCycles().Returns(EmptyList());

      //WHEN
      rule.Evaluate(AnyRoot.Root.Any.String(), cache, report);

      //THEN
      report.ReceivedNothing();
    }

    private static List<IReadOnlyList<string>> EmptyList()
    {
      return new List<IReadOnlyList<string>>();
    }
  }
}