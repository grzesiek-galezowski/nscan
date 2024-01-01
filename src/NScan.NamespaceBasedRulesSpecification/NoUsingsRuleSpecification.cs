using LanguageExt;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRulesSpecification;

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
    var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
    var report = Substitute.For<IAnalysisReportInProgress>();

    namespacesCache.RetrievePathsBetween(dto.FromPattern, dto.ToPattern)
      .Returns(NoPaths());

    //WHEN
    rule.Evaluate(Any.Instance<AssemblyName>(), namespacesCache, report);

    //THEN
    report.ReceivedNothing();
  }

  [Fact]
  public void ShouldReportErrorWhenThereIsADependency()
  {
    //GIVEN
    var dto = Any.Instance<NoUsingsRuleComplementDto>();
    var ruleViolationFactory = Substitute.For<INamespaceBasedRuleViolationFactory>();
    var assemblyName = Any.Instance<AssemblyName>();
    var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
    var report = Substitute.For<IAnalysisReportInProgress>();
    var violation = Any.Instance<RuleViolation>();
    var pathsFound = Arr.create(
      Any.Instance<NamespaceDependencyPath>(),
      Any.Instance<NamespaceDependencyPath>(),
      Any.Instance<NamespaceDependencyPath>()
    );
    var rule = new NoUsingsRule(dto, ruleViolationFactory);

    ruleViolationFactory.NoUsingsRuleViolation(rule.Description(), assemblyName, pathsFound)
      .Returns(violation);
    namespacesCache.RetrievePathsBetween(dto.FromPattern, dto.ToPattern).Returns(pathsFound);

    //WHEN
    rule.Evaluate(assemblyName, namespacesCache, report);

    //THEN
    report.Received(1).Add(violation);
  }

  private static Arr<NamespaceDependencyPath> NoPaths()
  {
    return Arr<NamespaceDependencyPath>.Empty;
  }
}
