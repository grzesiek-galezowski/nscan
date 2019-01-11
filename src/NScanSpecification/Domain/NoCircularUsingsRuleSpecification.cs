using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class NoCircularUsingsRuleSpecification
  {
    [Fact]
    public void ShouldReturnDescriptionBasedOnDto()
    {
      //GIVEN
      var dto = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var rule = new NoCircularUsingsRule(dto);

      //WHEN
      var description = rule.Description();

      //THEN
      description.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    [Fact]
    public void ShouldReportErrorWhenNamespacesCacheContainsAnyCycles()
    {
      //GIVEN
      var rule = new NoCircularUsingsRule(Any.Instance<NoCircularUsingsRuleComplementDto>());
      var cache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var cycles = Any.ReadOnlyList<IReadOnlyList<string>>();
      var projectAssemblyName = Any.String();

      cache.RetrieveCycles().Returns(cycles);

      //WHEN
      rule.Evaluate(projectAssemblyName, cache, report);

      //THEN
      report.Received(1).NamespacesBasedRuleViolation(rule.Description(), projectAssemblyName, cycles);
    }

    [Fact]
    public void ShouldNotReportErrorWhenNamespacesCacheContainsNoCycles()
    {
      //GIVEN
      var rule = new NoCircularUsingsRule(Any.Instance<NoCircularUsingsRuleComplementDto>());
      var cache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();

      cache.RetrieveCycles().Returns(EmptyList());

      //WHEN
      rule.Evaluate(Any.String(), cache, report);

      //THEN
      report.ReceivedNothing();
    }

    private static List<IReadOnlyList<string>> EmptyList()
    {
      return new List<IReadOnlyList<string>>();
    }
    //bug no error when no cycles

  }
}