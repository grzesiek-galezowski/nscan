using System.Linq;
using LanguageExt;
using NScan.Lib;
using NScan.NamespaceBasedRules;

namespace NScan.NamespaceBasedRulesSpecification;

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
    cycles.ToList().Should().BeEmpty();
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
    cycles.ToList().Should().BeEmpty();
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
    cycles.ToList().Should().BeEmpty();
  }

  [Fact]
  public void ShouldBeAbleToRetrieveSingleDirectCycle()
  {
    //GIVEN
    var cache = new NamespacesDependenciesCache();
    var namespace1 = new NamespaceName("a");
    var namespace2 = new NamespaceName("b");

    cache.AddMapping(namespace1, namespace2);
    cache.AddMapping(namespace2, namespace1);

    //WHEN
    var cycles = cache.RetrieveCycles();

    //THEN
    cycles.Single().Should().Be(new NamespaceDependencyPath(
      Seq.create(
        namespace1, 
        namespace2, 
        namespace1
      )));
  }

  [Fact]
  public void ShouldBeAbleToRetrieveSingleIndirectCycleTakingOrderIntoAccount()
  {
    //GIVEN
    var cache = new NamespacesDependenciesCache();
    var namespace1 = new NamespaceName("a");
    var namespace2 = new NamespaceName("b");
    var namespace3 = new NamespaceName("c");

    cache.AddMapping(namespace1, namespace2);
    cache.AddMapping(namespace2, namespace3);
    cache.AddMapping(namespace3, namespace1);

    //WHEN
    var cycles = cache.RetrieveCycles();

    //THEN
    cycles.Single().Should().Be(NamespaceDependencyPath.With(
      namespace1, 
      namespace2, 
      namespace3, 
      namespace1
    ));
  }

  [Fact]
  public void ShouldBeAbleToRetrieveMultipleIndirectCycleTakingOrderIntoAccount()
  {
    //GIVEN
    var cache = new NamespacesDependenciesCache();
    var namespace1 = new NamespaceName("a");
    var namespace2 = new NamespaceName("b");
    var namespace3 = new NamespaceName("c");
    var namespace4 = new NamespaceName("d");

    cache.AddMapping(namespace1, namespace2);
    cache.AddMapping(namespace2, namespace1);
      
    cache.AddMapping(namespace1, namespace3);
    cache.AddMapping(namespace3, namespace4);
    cache.AddMapping(namespace4, namespace1);

    //WHEN
    var cycles = cache.RetrieveCycles();

    //THEN
    cycles[0].Should().Be(NamespaceDependencyPath.With(
      namespace1,
      namespace2,
      namespace1
    ));
    cycles[1].Should().Be(NamespaceDependencyPath.With(
      namespace1,
      namespace3,
      namespace4,
      namespace1
    ));
    cycles.ToList().Should().HaveCount(2);
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
      Pattern.WithoutExclusion(namespace1.ToString()), 
      Pattern.WithoutExclusion(namespace4.ToString()));

    //THEN
    paths.ToList().Should().BeEmpty();
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
      Pattern.WithoutExclusion(namespace1.ToString()), 
      Pattern.WithoutExclusion(namespace1.ToString()));

    //THEN
    paths.ToList().Should().BeEmpty();
  }

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
      Pattern.WithoutExclusion(namespace1.ToString()), 
      Pattern.WithoutExclusion(namespace2.ToString()));

    //THEN
    paths[0].Should().Be(NamespaceDependencyPath.With(
      namespace1, 
      namespace2
    ));
    paths.ToList().Should().HaveCount(1);
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
    cache.AddMapping(namespace2, namespace3);
    cache.AddMapping(namespace1, namespace4);
    cache.AddMapping(namespace4, namespace3);

    //WHEN
    var paths = cache.RetrievePathsBetween(
      Pattern.WithoutExclusion(namespace1.ToString()), 
      Pattern.WithoutExclusion(namespace3.ToString()));

    //THEN
    paths[0].Should().Be(NamespaceDependencyPath.With(
      
      namespace1, 
      namespace2, 
      namespace3
    ));
    paths[1].Should().Be(
      NamespaceDependencyPath.With(
        namespace1,
        namespace4,
        namespace3
      ));
    paths.ToList().Should().HaveCount(2);
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
      Pattern.WithoutExclusion(namespace1.ToString()), 
      Pattern.WithoutExclusion(namespace2.ToString()));

    //THEN
    paths[0].Should().Be(NamespaceDependencyPath.With(
      namespace1, 
      namespace2
    ));
    paths.ToList().Should().HaveCount(1);
  }
}
