using FluentAssertions;
using GlobExpressions;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingId = Any.Instance<Pattern>();
      var dependencyId = Any.Instance<Glob>();
      
      //WHEN
      var independentRuleComplementDto = new IndependentRuleComplementDto()
      {
        DependencyType = RuleFactory.ProjectDependencyType,
        DependencyPattern = dependencyId,
        DependingPattern = dependingId
      };
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

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
      var dependingNamePattern = Any.Instance<Pattern>();
      var packageNamePattern = Any.Instance<Glob>();

      //WHEN
      var independentRuleComplementDto = new IndependentRuleComplementDto()
      {
        DependencyType = RuleFactory.PackageDependencyType,
        DependencyPattern = packageNamePattern,
        DependingPattern = dependingNamePattern
      };
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

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
      var dependingNamePattern = Any.Instance<Pattern>();
      var assemblyNamePattern = Any.Instance<Glob>();
      
      //WHEN
      var independentRuleComplementDto = new IndependentRuleComplementDto()
      {
        DependencyType = RuleFactory.AssemblyDependencyType,
        DependencyPattern = assemblyNamePattern,
        DependingPattern = dependingNamePattern
      };
      var rule = ruleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);

      //THEN
      rule.Should().BeOfType<IndependentRule>();
      rule.Should().DependOnTypeChain(typeof(DescribedCondition), typeof(HasAssemblyReferenceMatchingCondition));
      rule.Should().DependOn(dependingNamePattern);
      rule.Should().DependOn(assemblyNamePattern);
    }
  }
}