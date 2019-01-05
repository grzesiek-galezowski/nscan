using FluentAssertions;
using TddXt.NScan.CSharp;
using Xunit;

namespace TddXt.NScan.Specification.Integration
{
  //backlog single namesace per file rule


  public class RoslynNamespaceDetectionPlayground
  {
    [Fact]
    public void ShouldParseTopmostNonNestedNamespaces()
    {
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek {}").Should().Contain("Lolek").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek {} namespace Lolek {}").Should().Contain("Lolek").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek1 {} namespace Lolek2 {}").Should().Contain("Lolek1").And.Contain("Lolek2").And.HaveCount(2);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek.Lolek1 {}").Should().Contain("Lolek.Lolek1").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek { namespace Lolek1 {} }").Should().Contain("Lolek").And.Contain("Lolek1").And.HaveCount(2);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"").Should().BeEmpty(); //bug if file does not have any namespace, then NScan throws an exception
    }
  }
}
