using LanguageExt;
using NScan.NamespaceBasedRules;

namespace NScan.NamespaceBasedRulesSpecification;

public class SourceCodeFileUsingNamespacesSpecification
{
  [Fact]
  public void ShouldAddMappingBetweenNamespaceAndUsingsToCacheWhenAsked()
  {
    //GIVEN
    var namespace1 = Any.Instance<NamespaceName>();
    var namespace2 = Any.Instance<NamespaceName>();
    var namespace3 = Any.Instance<NamespaceName>();
    var using1 = Any.Instance<NamespaceName>();
    var using2 = Any.Instance<NamespaceName>();
    var using3 = Any.Instance<NamespaceName>();
    var cache = Substitute.For<INamespacesDependenciesCache>();

    var file = new SourceCodeFileUsingNamespaces(
      Seq.create(using1, using2, using3),
      Seq.create(namespace1, namespace2, namespace3)
    );

    //WHEN
    file.AddNamespaceMappingTo(cache);

    //THEN
    cache.Received(1).AddMapping(namespace1, using1);
    cache.Received(1).AddMapping(namespace1, using2);
    cache.Received(1).AddMapping(namespace1, using3);
    cache.Received(1).AddMapping(namespace2, using1);
    cache.Received(1).AddMapping(namespace2, using2);
    cache.Received(1).AddMapping(namespace2, using3);
    cache.Received(1).AddMapping(namespace3, using1);
    cache.Received(1).AddMapping(namespace3, using2);
    cache.Received(1).AddMapping(namespace3, using3);
  }
}
