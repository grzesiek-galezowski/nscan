﻿using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class ProjectDependencyPathSpecification
  {
    [Fact]
    public void ShouldReturnSegmentBetweenTwoResultsAsSegmentStartingWithDependingAndEndingWithDependency()
    {
      //GIVEN
      var projects = AnyRoot.Root.Any.ReadOnlyList<IReferencedProject>();
      var projectsSegment = AnyRoot.Root.Any.OtherThan(projects);
      var path = new ProjectDependencyPath(projects, AnyRoot.Root.Any.Instance<IProjectFoundSearchResultFactory>());
      var depending = Substitute.For<IProjectSearchResult>();
      var dependency = AnyRoot.Root.Any.Instance<IProjectSearchResult>();

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
      var project1 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project2 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project3 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var projects = new List<IDependencyPathBasedRuleTarget>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var depending = Substitute.For<IProjectSearchResult>();
      var condition = Substitute.For<IDescribedDependencyCondition>();
      var foundResult = AnyRoot.Root.Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      condition.Matches(depending, project1).Returns(false);
      condition.Matches(depending, project2).Returns(true);
      condition.Matches(depending, project3).Returns(false);
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
      var project1 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project2 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project3 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var projects = new List<IDependencyPathBasedRuleTarget>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var depending = Substitute.For<IProjectSearchResult>();
      var condition = Substitute.For<IDescribedDependencyCondition>();
      var notFoundResult = AnyRoot.Root.Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      condition.Matches(depending, project1).Returns(false);
      condition.Matches(depending, project2).Returns(false);
      condition.Matches(depending, project3).Returns(false);
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
      var pattern = AnyRoot.Root.Any.Instance<Pattern>();
      var project1 = Substitute.For<IReferencedProject>();
      var project2 = Substitute.For<IReferencedProject>();
      var project3 = Substitute.For<IReferencedProject>();
      var projects = new List<IDependencyPathBasedRuleTarget>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var foundResult = AnyRoot.Root.Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      project1.HasProjectAssemblyNameMatching(pattern).Returns(false);
      project2.HasProjectAssemblyNameMatching(pattern).Returns(true);
      project3.HasProjectAssemblyNameMatching(pattern).Returns(false);
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
      var pattern = AnyRoot.Root.Any.Instance<Pattern>();
      var project1 = Substitute.For<IReferencedProject>();
      var project2 = Substitute.For<IReferencedProject>();
      var project3 = Substitute.For<IReferencedProject>();
      var projects = new List<IDependencyPathBasedRuleTarget>
      {
        project1, project2, project3
      };
      var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
      var notFoundResult = AnyRoot.Root.Any.Instance<IProjectSearchResult>();

      var path = new ProjectDependencyPath(projects, searchResultFactory);

      project1.HasProjectAssemblyNameMatching(pattern).Returns(false);
      project2.HasProjectAssemblyNameMatching(pattern).Returns(false);
      project3.HasProjectAssemblyNameMatching(pattern).Returns(false);
      searchResultFactory.ItemNotFound().Returns(notFoundResult);

      //WHEN
      var actualResult = path.AssemblyWithNameMatching(pattern);

      //THEN
      actualResult.Should().Be(notFoundResult);
    }

  }
}