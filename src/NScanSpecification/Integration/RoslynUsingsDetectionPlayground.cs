using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using TddXt.NScan.CSharp;
using Xunit;

namespace TddXt.NScan.Specification.Integration
{
  public class RoslynUsingsDetectionPlayground
  {
    private static readonly IReadOnlyDictionary<string, ClassDeclarationInfo> NoClassDeclarations 
      = new Dictionary<string, ClassDeclarationInfo>();

    [Fact]
    public void ShouldGatherNormalUsingsFromAllLevels()
    {
      CSharpSyntax.GetAllUsingsFrom(@"
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace Lolek
{
  using Nunit;
  namespace Zenek
  {
    using Trolololo;
  }
}
", NoClassDeclarations).Should()
        .Contain("Microsoft.CodeAnalysis.CSharp")
        .And.Contain("Nunit")
        .And.Contain("Trolololo");
      //backlog does not support static usings...
    }

    [Fact]
    public void ShouldCorrectlyRecognizeAliases()
    {
      CSharpSyntax.GetAllUsingsFrom(@"using trolololo = Microsoft.CodeAnalysis.CSharp;", NoClassDeclarations)
        .Should().Contain("Microsoft.CodeAnalysis.CSharp");
    }

    [Fact]
    public void ShouldCorrectlyRecognizeLocalStaticUsings()
    {
      CSharpSyntax.GetAllUsingsFrom(
          @"using static TddXt.AnyRoot.Root;", 
          new Dictionary<string, ClassDeclarationInfo>()
          {
            {"TddXt.AnyRoot.Root", new ClassDeclarationInfo("Root", "TddXt.AnyRoot")}
          })
        .Should().Contain("TddXt.AnyRoot");
    }

    [Fact]
    public void ShouldIgnoreForeignStaticUsings()
    {
      CSharpSyntax.GetAllUsingsFrom(
          @"using static TddXt.AnyRoot.Root;", 
          NoClassDeclarations)
        .Should().BeEmpty();
    }
  }
}