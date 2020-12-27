using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.Lib;
using NScan.NamespaceBasedRules;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NamespacesDependenciesCacheSpecification
  {
    [Fact]
    public void ShouldReturnNoCyclesWhenNoMappingsWereAdded()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnNoCyclesWhenMappingsAddedDoNotMakeTheCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace3);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Should().BeEmpty();
    }
    
    [Fact]
    public void ShouldReturnNoCyclesWhenMappingExistsBetweenANamespaceAndItself()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Should().BeEmpty();
    }

    [Fact]
    public void ShouldBeAbleToRetrieveSingleDirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Single().Should().Equal(new List<NamespaceName>
      {
        namespace1, 
        namespace2, 
        namespace1
      });
    }

    [Fact]
    public void ShouldBeAbleToRetrieveSingleIndirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace3);
      cache.AddMapping(namespace3, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Single().Should().BeEquivalentTo(new List<NamespaceName>
      {
        namespace1, 
        namespace2, 
        namespace3, 
        namespace1
      });
    }

    [Fact]
    public void ShouldBeAbleToRetrieveMultipleIndirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();
      var namespace4 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace1);
      
      cache.AddMapping(namespace1, namespace3);
      cache.AddMapping(namespace3, namespace4);
      cache.AddMapping(namespace4, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles[0].Should().Equal(new List<NamespaceName>
      {
        namespace1,
        namespace2,
        namespace1,
      });
      cycles[1].Should().Equal(new List<NamespaceName>
      {
        namespace1, 
        namespace3, 
        namespace4, 
        namespace1
      });
      cycles.Should().HaveCount(2);
    }
    
    [Fact]
    public void ShouldReturnEmptyListWhenThereAreNoPathsFromSourceToDestination()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();
      var namespace4 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace3);
      cache.AddMapping(namespace4, namespace1);

      //WHEN
      var paths = cache.RetrievePathsBetween(
        Pattern.WithoutExclusion(namespace1.Value), 
        Pattern.WithoutExclusion(namespace4.Value));

      //THEN
      paths.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotReturnAPathConsistingOfASingleEntry()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);

      //WHEN
      var paths = cache.RetrievePathsBetween(
        Pattern.WithoutExclusion(namespace1.Value), 
        Pattern.WithoutExclusion(namespace1.Value));

      //THEN
      paths.Should().BeEmpty();
    }

    //bug one path should be reported only once

    [Fact]
    public void ShouldReturnADirectPathFromSourceToDestination()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);

      //WHEN
      var paths = cache.RetrievePathsBetween(
        Pattern.WithoutExclusion(namespace1.Value), 
        Pattern.WithoutExclusion(namespace2.Value));

      //THEN
      paths[0].Should().BeEquivalentTo(new List<NamespaceName>
      {
        namespace1, 
        namespace2
      });
      paths.Should().HaveCount(1);
    }
    
    [Fact]
    public void ShouldReturnAllPathsFromSourceToDestinationWhenMultiplePathsExist()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();
      var namespace4 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace1, namespace4);
      cache.AddMapping(namespace2, namespace3);
      cache.AddMapping(namespace4, namespace3);

      //WHEN
      var paths = cache.RetrievePathsBetween(
        Pattern.WithoutExclusion(namespace1.Value), 
        Pattern.WithoutExclusion(namespace3.Value));

      //THEN
      paths[0].Should().BeEquivalentTo(new List<NamespaceName>
      {
        namespace1, 
        namespace2, 
        namespace3
      });
      paths[1].Should().BeEquivalentTo(
        new List<NamespaceName>
        {
          namespace1, 
          namespace4, 
          namespace3
        });
      paths.Should().HaveCount(2);
    }
    
    [Fact]
    public void ShouldReduceDuplicatesWhenReturningPaths()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace1, namespace2);

      //WHEN
      var paths = cache.RetrievePathsBetween(
        Pattern.WithoutExclusion(namespace1.Value), 
        Pattern.WithoutExclusion(namespace2.Value));

      //THEN
      paths[0].Should().BeEquivalentTo(new List<NamespaceName>
      {
        namespace1, 
        namespace2
      });
      paths.Should().HaveCount(1);
    }
  }
}
