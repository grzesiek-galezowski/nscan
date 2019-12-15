using System;
using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScan.Domain;
using NScanSpecification.Lib;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
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
      new Action(() => projects.Put(projectId, dependencyPathBasedRuleTarget)).Should().NotThrow();
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
      new Action(() => projects.Put(projectId, Any.OtherThan(dependencyPathBasedRuleTarget))).Should().ThrowExactly<ProjectShadowingException>();
    }

    
  }
}