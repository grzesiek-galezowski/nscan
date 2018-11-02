using FluentAssertions;
using GlobExpressions;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingId = Any.Instance<Glob>();
      var dependencyId = Any.Instance<Glob>();
      
      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(
        new RuleDto()
        {
          DependencyPattern = dependencyId,
          DependingPattern = dependingId,
          DependencyType = RuleFactory.ProjectDependencyType
        });

      //THEN
      rule.GetType().Should().Be<IndependentRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOnTypeChain(typeof(JoinedDescribedCondition), typeof(IsFollowingAssemblyCondition));
      rule.Should().DependOnTypeChain(typeof(JoinedDescribedCondition), typeof(HasAssemblyNameMatchingPatternCondition));
      rule.Should().DependOn(dependencyId);
    }

    [Fact]
    public void ShouldCreateIndependentOfPackageRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingNamePattern = Any.Instance<Glob>();
      var packageNamePattern = Any.Instance<Glob>();
      
      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(
        new RuleDto()
        {
          DependencyPattern = packageNamePattern,
          DependingPattern = dependingNamePattern,
          DependencyType = RuleFactory.PackageDependencyType
        });

      //THEN
      rule.GetType().Should().Be<IndependentRule>();
      rule.Should().DependOnTypeChain(typeof(DescribedCondition), typeof(HasPackageReferenceMatchingCondition));
      rule.Should().DependOn(dependingNamePattern);
      rule.Should().DependOn(packageNamePattern);
    }
    
    [Fact]
    public void ShouldCreateIndependentOfAssemblyRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingNamePattern = Any.Instance<Glob>();
      var assemblyNamePattern = Any.Instance<Glob>();
      
      //WHEN
      var rule = ruleFactory.CreateDependencyRuleFrom(
        new RuleDto()
        {
          DependencyPattern = assemblyNamePattern,
          DependingPattern = dependingNamePattern,
          DependencyType = RuleFactory.AssemblyDependencyType
        });

      //THEN
      rule.Should().BeOfType<IndependentRule>();
      rule.Should().DependOnTypeChain(typeof(DescribedCondition), typeof(HasAssemblyReferenceMatchingCondition));
      rule.Should().DependOn(dependingNamePattern);
      rule.Should().DependOn(assemblyNamePattern);
    }
  }
}