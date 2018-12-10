/*using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TddXt.NScan.Specification.Integration
{
  public static class CSharpSyntax
  {
    public static IEnumerable<string> GetAllUniqueNamespacesFrom(string fileText)
    {
      var tree = CSharpSyntaxTree.ParseText(fileText);

      return tree.GetCompilationUnitRoot().Members.Where(MemberIsNamespace())
        .Cast<NamespaceDeclarationSyntax>().Select(NamespaceName()).ToHashSet<string>();
    }

    private static Func<NamespaceDeclarationSyntax, string> NamespaceName()
    {
      return ns => ns.Name.ToString();
    }

    private static Func<MemberDeclarationSyntax, bool> MemberIsNamespace()
    {
      return m => m is NamespaceDeclarationSyntax;
    }
  }

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
} bug
*/