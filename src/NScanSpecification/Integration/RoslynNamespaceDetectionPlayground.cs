using FluentAssertions;
using TddXt.NScan.Xml;
using Xunit;

namespace TddXt.NScan.Specification.Integration
{
  

  public class RoslynNamespaceDetectionPlayground
  {
    [Fact]
    public void ShouldParseTopmostNonNestedNamespaces()
    {
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek {}").Should().Contain("Lolek").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek {} namespace Lolek {}").Should().Contain("Lolek").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek1 {} namespace Lolek2 {}").Should().Contain("Lolek1").And.Contain("Lolek2").And.HaveCount(2);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek.Lolek1 {}").Should().Contain("Lolek.Lolek1").And.HaveCount(1);
      CSharpSyntax.GetAllUniqueNamespacesFrom(@"namespace Lolek { namespace Lolek1 {} }").Should().Contain("Lolek").And.HaveCount(1);
    }
  }
}
