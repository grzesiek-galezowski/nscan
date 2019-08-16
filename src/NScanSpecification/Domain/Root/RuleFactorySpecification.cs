using FluentAssertions;
using GlobExpressions;
using NScan.Domain.DependencyPathBasedRules;
using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;
using NScan.Domain.Root;
using NScan.SharedKernel.RuleDtos;
using NScanSpecification.Lib;
using TddXt.NScan.Specification.Domain.ProjectScopedRules;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var dependingId = Any.Pattern();
      var dependencyId = Any.Instance<Glob>();
      
      //WHEN
      var independentRuleComplementDto =
        new IndependentRuleComplementDto(
          RuleFactory.ProjectDependencyType, 
          dependingId, 
          dependencyId);
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
      var dependingNamePattern = Any.Pattern();
      var packageNamePattern = Any.Instance<Glob>();

      //WHEN
      var independentRuleComplementDto = new IndependentRuleComplementDto(
        RuleFactory.PackageDependencyType,
        dependingNamePattern, 
        packageNamePattern);
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
      var dependingNamePattern = Any.Pattern();
      var assemblyNamePattern = Any.Instance<Glob>();
      
      //WHEN
      var independentRuleComplementDto = new IndependentRuleComplementDto(
        RuleFactory.AssemblyDependencyType, 
        dependingNamePattern, 
        assemblyNamePattern);
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
      var ruleDto = Any.Instance<CorrectNamespacesRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<ProjectScopedRuleApplicableToMatchingProject>();
      projectScopedRule.Should().DependOn<ProjectSourceCodeFilesRelatedRule>();
      projectScopedRule.Should().DependOn(ruleDto.ProjectAssemblyNamePattern);
    }

    [Fact]
    public void ShouldCreateNoCircularDependenciesRule()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var ruleDto = Any.Instance<NoCircularUsingsRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<NoCircularUsingsRule>();
      projectScopedRule.Should().DependOn(ruleDto);
    }

    [Fact]
    public void ShouldCreateHasAttributesOnRuleFromDto()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var ruleDto = Any.Instance<HasAttributesOnRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<ProjectScopedRuleApplicableToMatchingProject>();
      projectScopedRule.Should().DependOn<ProjectSourceCodeFilesRelatedRule>();
      projectScopedRule.Should().DependOn<MethodsOfMatchingClassesAreDecoratedWithAttributeCheck>();
      projectScopedRule.Should().DependOn(ruleDto.ClassNameInclusionPattern);
      projectScopedRule.Should().DependOn(ruleDto.MethodNameInclusionPattern);
    }

    [Fact]
    public void ShouldCreateHasTargetFrameworkRuleFromDto()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      var ruleDto = Any.Instance<HasTargetFrameworkRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<ProjectScopedRuleApplicableToMatchingProject>();
      projectScopedRule.Should().DependOn<HasTargetFrameworkRule>();
      projectScopedRule.Should().DependOn(ruleDto.ProjectAssemblyNamePattern);
      projectScopedRule.Should().DependOn(ruleDto.TargetFramework);
    }
  }
}