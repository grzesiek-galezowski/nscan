using System;
using System.Collections.Generic;
using MyTool.App;
using NSubstitute;
using TddXt.AnyExtensibility;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyToolSpecification
{
  public class DotNetStandardSolutionSpecification
  {
    [Fact]
    public void ShouldResolveReferencesOfAllProjectUsingItselfAsAContext()
    {
      //GIVEN
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        { Any.ProjectId(), project1 },
        { Any.ProjectId(), project2 },
        { Any.ProjectId(), project3 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById);
      

      //WHEN
      dotNetStandardSolution.ResolveAllProjectsReferences();

      //THEN
      Received.InOrder(() =>
      {
        project1.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
        project2.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
        project3.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
      });
    }

    [Fact]
    public void ShouldAddTwoWayBindingBetweenReferencedAndReferencingProjectDuringResolutionFromSolution()
    {
      //GIVEN
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project1Id = Any.ProjectId();
      var project2Id = Any.ProjectId();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { project1Id, project1 },
        { project2Id, project2 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById);
      

      //WHEN
      dotNetStandardSolution.ResolveReferenceFrom(project1, project2Id);

      //THEN
      project1.Received(1).ResolveAsReferencing(project2);
      project2.Received(1).ResolveAsReferenceOf(project1);
    }
  }


  public static class MyAnyExtensions
  {
    public static ProjectId ProjectId(this BasicGenerator gen)
    {
      return gen.Instance<ProjectId>();
    }
  }
}
