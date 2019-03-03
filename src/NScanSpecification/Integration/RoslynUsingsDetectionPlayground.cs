﻿using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using TddXt.NScan.ReadingCSharpSourceCode;
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
      CSharpFileSyntaxTree.ParseText(@"
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
", "").GetAllUsingsFrom(NoClassDeclarations).Should()
        .Contain("Microsoft.CodeAnalysis.CSharp")
        .And.Contain("Nunit")
        .And.Contain("Trolololo");
      //backlog does not support static usings...
    }

    [Fact]
    public void ShouldCorrectlyRecognizeAliases()
    {
      CSharpFileSyntaxTree.ParseText(@"using trolololo = Microsoft.CodeAnalysis.CSharp;", "").GetAllUsingsFrom(NoClassDeclarations)
        .Should().Contain("Microsoft.CodeAnalysis.CSharp");
    }

    [Fact]
    public void ShouldCorrectlyRecognizeLocalStaticUsings()
    {
      CSharpFileSyntaxTree.ParseText(@"using static TddXt.AnyRoot.Root;", "").GetAllUsingsFrom(new Dictionary<string, ClassDeclarationInfo>()
          {
            {"TddXt.AnyRoot.Root", new ClassDeclarationInfo("Root", "TddXt.AnyRoot")}
          })
        .Should().Contain("TddXt.AnyRoot");
    }

    [Fact]
    public void ShouldIgnoreForeignStaticUsings()
    {
      CSharpFileSyntaxTree.ParseText(@"using static TddXt.AnyRoot.Root;", "").GetAllUsingsFrom(NoClassDeclarations)
        .Should().BeEmpty();
    }
  }
}