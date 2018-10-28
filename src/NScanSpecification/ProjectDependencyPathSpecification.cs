﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class ProjectDependencyPathSpecification
  {
    [Fact]
    public void ShouldReturnSegmentBetweenTwoResultsAsSegmentStartingWithDependingAndEndingWithDependency()
    {
      //GIVEN
      var projects = Any.ReadOnlyList<IReferencedProject>();
      var projectsSegment = Any.OtherThan(projects);
      var path = new ProjectDependencyPath(projects, Any.Instance<IProjectFoundSearchResultFactory>());
      var depending = Substitute.For<IProjectSearchResult>();
      var dependency = Any.Instance<IProjectSearchResult>();

      depending.SegmentEndingWith(dependency, projects).Returns(projectsSegment);

      //WHEN
      var actualSegment = path.SegmentBetween(depending, dependency);
      
      //THEN
      actualSegment.Should().BeEquivalentTo(projectsSegment);
    }
    
    [Fact]
    public void ShouldReturnResultWithFoundProjectAndItsIndexWhenTheProjectMatchesCondition()
    {
      //GIVEN
      var project1 = Any.Instance<IReferencedProject>();
      var project2 = Any.Instance<IReferencedProject>();
      var project3 = Any.Instance<IReferencedProject>();
      var projects = new List<IReferencedProject>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var depending = Substitute.For<IProjectSearchResult>();
      var condition = Substitute.For<IDependencyCondition>();
      var foundResult = Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      condition.Matches(project1, depending).Returns(false);
      condition.Matches(project2, depending).Returns(true);
      condition.Matches(project3, depending).Returns(false);
      searchResultFactory.ItemFound(project2, 1).Returns(foundResult);

      //WHEN
      var actualResult = path.AssemblyMatching(condition, depending);
      
      //THEN
      actualResult.Should().Be(foundResult);
    }
    
    [Fact]
    public void ShouldReturnNotFoundResultWhenNoneOfTheProjectsMatchCondition()
    {
      //GIVEN
      var project1 = Any.Instance<IReferencedProject>();
      var project2 = Any.Instance<IReferencedProject>();
      var project3 = Any.Instance<IReferencedProject>();
      var projects = new List<IReferencedProject>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var depending = Substitute.For<IProjectSearchResult>();
      var condition = Substitute.For<IDependencyCondition>();
      var notFoundResult = Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      condition.Matches(project1, depending).Returns(false);
      condition.Matches(project2, depending).Returns(false);
      condition.Matches(project3, depending).Returns(false);
      searchResultFactory.ItemNotFound().Returns(notFoundResult);

      //WHEN
      var actualResult = path.AssemblyMatching(condition, depending);
      
      //THEN
      actualResult.Should().Be(notFoundResult);
    }

    [Fact]
    public void ShouldReturnResultWithFoundProjectAndItsIndexWhenTheProjectMatchesNamePattern()
    {
      //GIVEN
      var project1 = Substitute.For<IReferencedProject>();
      var project2 = Substitute.For<IReferencedProject>();
      var project3 = Substitute.For<IReferencedProject>();
      var projects = new List<IReferencedProject>
      {
        project1, project2, project3
      };
      var pattern = Any.String();
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var foundResult = Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      project1.HasAssemblyNameMatching(pattern).Returns(false);
      project2.HasAssemblyNameMatching(pattern).Returns(true);
      project3.HasAssemblyNameMatching(pattern).Returns(false);
      searchResultFactory.ItemFound(project2, 1).Returns(foundResult);

      //WHEN
      var actualResult = path.AssemblyWithNameMatching(pattern);

      //THEN
      actualResult.Should().Be(foundResult);
    }

    [Fact]
    public void ShouldReturnNotFoundResultWhenNoneOfTheProjectsMatchNamePattern()
    {
      //GIVEN
      var project1 = Substitute.For<IReferencedProject>();
      var project2 = Substitute.For<IReferencedProject>();
      var project3 = Substitute.For<IReferencedProject>();
      var projects = new List<IReferencedProject>
      {
        project1, project2, project3
      };
      var pattern = Any.String();
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var notFoundResult = Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      project1.HasAssemblyNameMatching(pattern).Returns(false);
      project2.HasAssemblyNameMatching(pattern).Returns(false);
      project3.HasAssemblyNameMatching(pattern).Returns(false);
      searchResultFactory.ItemNotFound().Returns(notFoundResult);

      //WHEN
      var actualResult = path.AssemblyWithNameMatching(pattern);

      //THEN
      actualResult.Should().Be(notFoundResult);
    }

  }
}