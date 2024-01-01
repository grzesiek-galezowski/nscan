using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;

namespace NScan.NamespaceBasedRulesSpecification;

public class NamespaceBasedRuleViolationFactorySpecification
{
  [Fact]
  public void ShouldCreateAViolationForNoCyclesRuleContainingCyclesDescription()
  {
    //GIVEN
    var fragments = Substitute.For<INamespaceBasedReportFragmentsFormat>();
    var factory = new NamespaceBasedRuleViolationFactory(fragments);
    var cyclesString = Any.String();
    var description = Any.Instance<RuleDescription>();
    var projectAssemblyName = Any.Instance<AssemblyName>();
    var cycles = Any.Arr<NamespaceDependencyPath>();

    fragments.ApplyTo(cycles, "Cycle").Returns(cyclesString);
      
    //WHEN
    var violation = factory.NoCyclesRuleViolation(description, projectAssemblyName, cycles);

    //THEN
    violation.Should().Be(RuleViolation.Create(description,
      $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}",
      cyclesString));
  }
    
  [Fact]
  public void ShouldCreateAViolationForNoUsingsRuleContainingPathsDescription()
  {
    //GIVEN
    var fragments = Substitute.For<INamespaceBasedReportFragmentsFormat>();
    var factory = new NamespaceBasedRuleViolationFactory(fragments);
    var pathsString = Any.String();
    var description = Any.Instance<RuleDescription>();
    var projectAssemblyName = Any.Instance<AssemblyName>();
    var paths = Any.Arr<NamespaceDependencyPath>();

    fragments.ApplyTo(paths, "Violation").Returns(pathsString);
      
    //WHEN
    var violation = factory.NoUsingsRuleViolation(description, projectAssemblyName, paths);

    //THEN
    violation.Should().Be(RuleViolation.Create(description,
      $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}",
      pathsString));
  }
}
