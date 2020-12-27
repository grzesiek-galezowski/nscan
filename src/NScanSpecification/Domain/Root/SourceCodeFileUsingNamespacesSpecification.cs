using System.Collections.Generic;
using NScan.NamespaceBasedRules;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class SourceCodeFileUsingNamespacesSpecification
  {
    [Fact]
    public void ShouldAddMappingBetweenNamespaceAndUsingsToCacheWhenAsked()
    {
      //GIVEN
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      var namespace3 = Any.String();
      var using1 = Any.String();
      var using2 = Any.String();
      var using3 = Any.String();
      var cache = Substitute.For<INamespacesDependenciesCache>();

      var file = new SourceCodeFileUsingNamespaces(
        new List<string> {using1, using2, using3},
        new List<string> {namespace1, namespace2, namespace3}
      );

      //WHEN
      file.AddNamespaceMappingTo(cache);

      //THEN
      cache.Received(1).AddMapping(
        new NamespaceName(namespace1), new NamespaceName(using1));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace1), new NamespaceName(using2));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace1), new NamespaceName(using3));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace2), new NamespaceName(using1));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace2), new NamespaceName(using2));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace2), new NamespaceName(using3));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace3), new NamespaceName(using1));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace3), new NamespaceName(using2));
      cache.Received(1).AddMapping(
        new NamespaceName(namespace3), new NamespaceName(using3));
    }
  }
}
