﻿namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution.Integration;
//backlog single namespace per file rule

public class RoslynNamespaceDetectionPlayground
{
  [Fact]
  public void ShouldParseTopmostNonNestedNamespaces()
  {
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek {}", "").GetAllUniqueNamespaces().Should().Contain("Lolek").And.HaveCount(1);
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek {} namespace Lolek {}", "").GetAllUniqueNamespaces().Should().Contain("Lolek").And.HaveCount(1);
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek1 {} namespace Lolek2 {}", "").GetAllUniqueNamespaces().Should().Contain("Lolek1").And.Contain("Lolek2").And.HaveCount(2);
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek.Lolek1 {}", "").GetAllUniqueNamespaces().Should().Contain("Lolek.Lolek1").And.HaveCount(1);
    CSharpFileSyntaxTree.ParseText(@"", "").GetAllUniqueNamespaces().Should().BeEmpty();
  }

  [Fact]
  public void ShouldParseNestedNamespacesInAWrongWay() //this is a bug - not implemented yet
  {
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek { namespace Lolek1 {} }", "").GetAllUniqueNamespaces().Should().Equal("Lolek", "Lolek1").And.HaveCount(2);
  }

  [Fact]
  public void ShouldParseFileScopedNamespaces()
  {
    CSharpFileSyntaxTree.ParseText(@"namespace Lolek;", "").GetAllUniqueNamespaces().Should().Equal("Lolek");
  }
}
