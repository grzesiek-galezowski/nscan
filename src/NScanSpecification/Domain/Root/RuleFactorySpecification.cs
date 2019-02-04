using FluentAssertions;
using GlobExpressions;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingId = AnyRoot.Root.Any.Instance<Pattern>();
      var dependencyId = AnyRoot.Root.Any.Instance<Glob>();
      
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
      var dependingNamePattern = AnyRoot.Root.Any.Instance<Pattern>();
      var packageNamePattern = AnyRoot.Root.Any.Instance<Glob>();

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
      var dependingNamePattern = AnyRoot.Root.Any.Instance<Pattern>();
      var assemblyNamePattern = AnyRoot.Root.Any.Instance<Glob>();
      
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

    [Fact]
    public void ShouldCreateCorrectNamespacesRule()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var ruleDto = AnyRoot.Root.Any.Instance<CorrectNamespacesRuleComplementDto>();


      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<CorrectNamespacesRule>();
      projectScopedRule.Should().DependOn(ruleDto);

    }

    [Fact]
    public void ShouldCreateNoCircularDependenciesRule()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var ruleDto = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<NoCircularUsingsRule>();
      projectScopedRule.Should().DependOn(ruleDto);

    }
  }
}