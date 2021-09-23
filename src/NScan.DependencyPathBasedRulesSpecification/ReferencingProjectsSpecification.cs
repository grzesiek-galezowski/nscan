using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScanSpecification.Lib;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class ReferencingProjectsSpecification
  {
    [Fact]
    public void ShouldSayItIsEmptyWhenItHasNoReferencingProjects()
    {
      //GIVEN
      var projects = new ReferencingProjects();

      //WHEN
      var areEmpty = projects.AreEmpty();

      //THEN
      areEmpty.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItIsNotEmptyWhenItHasAtLeastOneReferencingProject()
    {
      //GIVEN
      var projects = new ReferencingProjects();
      projects.Put(Any.ProjectId(), Any.Instance<IDependencyPathBasedRuleTarget>());

      //WHEN
      var areEmpty = projects.AreEmpty();

      //THEN
      areEmpty.Should().BeFalse();
    }

    [Fact]
    public void ShouldAllowAddingTheSameProjectTwice()
    {
      //GIVEN
      var projects = new ReferencingProjects();
      var projectId = Any.ProjectId();
      var dependencyPathBasedRuleTarget = Any.Instance<IDependencyPathBasedRuleTarget>();
      projects.Put(projectId, dependencyPathBasedRuleTarget);

      //WHEN - THEN
      projects.Invoking(p => p.Put(projectId, dependencyPathBasedRuleTarget)).Should().NotThrow();
    }

    [Fact]
    public void ShouldNotAllowAddingAnotherProjectUsingTheSameId()
    {
      //GIVEN
      var projects = new ReferencingProjects();
      var projectId = Any.ProjectId();
      var dependencyPathBasedRuleTarget = Any.Instance<IDependencyPathBasedRuleTarget>();
      projects.Put(projectId, dependencyPathBasedRuleTarget);

      //WHEN - THEN
      projects.Invoking(p => p.Put(projectId, Any.OtherThan(dependencyPathBasedRuleTarget)))
        .Should().ThrowExactly<ProjectShadowingException>();
    }

    
  }
}
