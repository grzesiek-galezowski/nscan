using System.Collections.Generic;
using FluentAssertions;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NoUsingsRuleSpecification
  {
    [Fact]
    public void ShouldReturnDescriptionBasedOnDto()
    {
      //GIVEN
      var dto = Any.Instance<NoUsingsRuleComplementDto>();
      var rule = new NoUsingsRule(dto, Any.Instance<INamespaceBasedRuleViolationFactory>());

      //WHEN
      var description = rule.Description();

      //THEN
      description.Should().Be(HasNoUsingsRuleMetadata.Format(dto));
    }

    [Fact]
    public void ShouldNotReportAnythingWhenThereIsNoDependency()
    {
      //GIVEN
      var dto = Any.Instance<NoUsingsRuleComplementDto>();
      var rule = new NoUsingsRule(dto, Any.Instance<INamespaceBasedRuleViolationFactory>());
      var assemblyName = Any.String();
      var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();

      namespacesCache.RetrievePathsBetween(dto.FromPattern, dto.ToPattern).Returns(new List<IReadOnlyList<string>>());

      //WHEN
      rule.Evaluate(assemblyName, namespacesCache, report);

      //THEN
      report.ReceivedNothing();
    }

    [Fact]
    public void ShouldReportErrorWhenThereIsADependency()
    {
      //GIVEN
      var dto = Any.Instance<NoUsingsRuleComplementDto>();
      var ruleViolationFactory = Substitute.For<INamespaceBasedRuleViolationFactory>();
      var rule = new NoUsingsRule(dto, ruleViolationFactory);
      var assemblyName = Any.String();
      var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var violation = Any.Instance<RuleViolation>();
      var pathsFound = new List<IReadOnlyList<string>>
      {
        Any.Instance<IReadOnlyList<string>>(),
        Any.Instance<IReadOnlyList<string>>(),
        Any.Instance<IReadOnlyList<string>>(),
      };
      ruleViolationFactory.NoUsingsRuleViolation(rule.Description(), assemblyName, pathsFound)
        .Returns(violation);
      namespacesCache.RetrievePathsBetween(dto.FromPattern, dto.ToPattern).Returns(pathsFound);

      //WHEN
      rule.Evaluate(assemblyName, namespacesCache, report);

      //THEN
      report.Received(1).Add(violation);
    }
    /*
      if (cycles.Any())
      {
        report.Add(_ruleViolationFactory.NoCyclesRuleViolation(Description(), projectAssemblyName, cycles));
      }
    */
    //bug support more paths than just one
  }
}