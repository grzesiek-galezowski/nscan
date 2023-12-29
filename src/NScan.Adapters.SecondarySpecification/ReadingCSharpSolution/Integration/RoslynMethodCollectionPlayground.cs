﻿using System.Collections.Generic;
using LanguageExt;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution.Integration;

public class RoslynMethodCollectionPlayground
{
  [Fact]
  public void ShouldGatherMethodsWithAttributesFromSourceCode()
  {
    //GIVEN
    var dictionary = CSharpFileSyntaxTree.ParseText(@"
namespace Namespace1.Namespace2
{
  class Class1
  {
    class Class2
    {
      public void Lol1() {}
      [Test1, Test2]
      [Test3, Test4]
      private void Lol2() {}
      private static void Lol3() {}
    }
  }
}
", "").GetClassDeclarationSignatures();

    dictionary["Namespace1.Namespace2.Class1.Class2"].Methods.Should().BeEquivalentTo(
      Seq.create(
        new MethodDeclarationInfo("Lol1", new List<string>()),
        new MethodDeclarationInfo("Lol2", new List<string> { "Test1", "Test2", "Test3", "Test4" }),
        new MethodDeclarationInfo("Lol3", new List<string>())
      ));
  }
}
