using FluentAssertions;
using GlobExpressions;
using NScan.DependencyPathBasedRules;
using NScan.Domain.Root;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScanSpecification.Lib;
using TddXt.XFluentAssert.Root;
using TddXt.XFluentAssertRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class DependencyPathRuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleViolationFactory = Any.Instance<IDependencyPathRuleViolationFactory>();
      var ruleFactory = new DependencyPathRuleFactory(ruleViolationFactory);
      var dependingId = Any.Pattern();
      var dependencyId = Any.Instance<Glob>();
      var independentRuleComplementDto =
        new IndependentRuleComplementDto(
          DependencyPathRuleFactory.ProjectDependencyType,
          dependingId,
          dependencyId);

      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

      //THEN
      rule.GetType().Should().Be<IndependentRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOnTypeChain(typeof(JoinedDescribedCondition), typeof(IsFollowingAssemblyCondition));
      rule.Should()
        .DependOnTypeChain(typeof(JoinedDescribedCondition), typeof(HasAssemblyNameMatchingPatternCondition));
      rule.Should().DependOn(dependencyId);
      rule.Should().DependOn(ruleViolationFactory);
    }

    [Fact]
    public void ShouldCreateIndependentOfPackageRuleWithPassedIds()
    {
      //GIVEN
      var violationFactory = Any.Instance<IDependencyPathRuleViolationFactory>();
      var ruleFactory = new DependencyPathRuleFactory(violationFactory);
      var dependingNamePattern = Any.Pattern();
      var packageNamePattern = Any.Instance<Glob>();
      var independentRuleComplementDto = new IndependentRuleComplementDto(
        DependencyPathRuleFactory.PackageDependencyType,
        dependingNamePattern,
        packageNamePattern);

      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

      //THEN
      rule.GetType().Should().Be<IndependentRule>();
      rule.Should().DependOnTypeChain(typeof(DescribedCondition), typeof(HasPackageReferenceMatchingCondition));
      rule.Should().DependOn(dependingNamePattern);
      rule.Should().DependOn(packageNamePattern);
      rule.Should().DependOn(violationFactory);
    }

    [Fact]
    public void ShouldCreateIndependentOfAssemblyRuleWithPassedIds()
    {
      //GIVEN
      var ruleViolationFactory = Any.Instance<IDependencyPathRuleViolationFactory>();
      var ruleFactory = new DependencyPathRuleFactory(ruleViolationFactory);
      var dependingNamePattern = Any.Pattern();
      var assemblyNamePattern = Any.Instance<Glob>();
      var independentRuleComplementDto = new IndependentRuleComplementDto(
        DependencyPathRuleFactory.AssemblyDependencyType,
        dependingNamePattern,
        assemblyNamePattern);

      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

      //THEN
      rule.Should().BeOfType<IndependentRule>();
      rule.Should().DependOnTypeChain(typeof(DescribedCondition), typeof(HasAssemblyReferenceMatchingCondition));
      rule.Should().DependOn(dependingNamePattern);
      rule.Should().DependOn(assemblyNamePattern);
      rule.Should().DependOn(ruleViolationFactory);
    }
  }
}