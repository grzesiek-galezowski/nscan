using FluentAssertions;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using TddXt.XFluentAssertRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class ProjectScopedRuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateCorrectNamespacesRule()
    {
      //GIVEN
      var ruleFactory = new ProjectScopedRuleFactory(Any.Instance<IProjectScopedRuleViolationFactory>());
      var ruleDto = Any.Instance<CorrectNamespacesRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<ProjectScopedRuleApplicableToMatchingProject>();
      projectScopedRule.Should().DependOn<ProjectSourceCodeFilesRelatedRule>();
      projectScopedRule.Should().DependOn(ruleDto.ProjectAssemblyNamePattern);
    }

    [Fact]
    public void ShouldCreateHasAttributesOnRuleFromDto()
    {
      //GIVEN
      var ruleFactory = new ProjectScopedRuleFactory(Any.Instance<IProjectScopedRuleViolationFactory>());
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
      var ruleViolationFactory = Any.Instance<IProjectScopedRuleViolationFactory>();
      var ruleFactory = new ProjectScopedRuleFactory(ruleViolationFactory);
      var ruleDto = Any.Instance<HasTargetFrameworkRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateProjectScopedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<ProjectScopedRuleApplicableToMatchingProject>();
      projectScopedRule.Should().DependOn<HasTargetFrameworkRule>();
      projectScopedRule.Should().DependOn(ruleDto.ProjectAssemblyNamePattern);
      projectScopedRule.Should().DependOn(ruleDto.TargetFramework);
      projectScopedRule.Should().DependOn(ruleViolationFactory);
    }
  }
}