﻿namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution.Integration;

public class RoslynClassCollectionPlayground
{
  [Fact]
  public void ShouldGatherClassesWithTheirEnclosingNamespaces()
  {
    var dictionary = CSharpFileSyntaxTree.ParseText(@"
using static Namespace1.Namespace2.Class1.Class2;

class GlobalClass
{
  class GlobalNestedClass
  { 
  }
}

namespace Namespace1.Namespace2
{
  class Class1
  {
    class Class2
    {
    }
  }
}
", "").GetClassDeclarationSignatures();

    dictionary["Namespace1.Namespace2.Class1"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1"].Name.Should().Be("Class1");
    dictionary["Namespace1.Namespace2.Class1.Class2"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1.Class2"].Name.Should().Be("Class1.Class2");
    dictionary["GlobalClass"].Namespace.Should().Be("");
    dictionary["GlobalClass"].Name.Should().Be("GlobalClass");
    dictionary["GlobalClass.GlobalNestedClass"].Namespace.Should().Be("");
    dictionary["GlobalClass.GlobalNestedClass"].Name.Should().Be("GlobalClass.GlobalNestedClass");
  }
  
  [Fact]
  public void ShouldGatherClassesWithTheirFileScopedNamespaces()
  {
    var dictionary = CSharpFileSyntaxTree.ParseText(@"
namespace Namespace1.Namespace2;

class Class1
{
  class Class2
  {
  }
}
", "").GetClassDeclarationSignatures();

    dictionary["Namespace1.Namespace2.Class1"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1"].Name.Should().Be("Class1");
    dictionary["Namespace1.Namespace2.Class1.Class2"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1.Class2"].Name.Should().Be("Class1.Class2");
  }
    
  [Fact]
  public void ShouldGatherBeAbleToDistinguishBetweenNonGenericClassAndGenericClassWithTheSameName()
  {
    var dictionary = CSharpFileSyntaxTree.ParseText(@"
namespace Namespace1.Namespace2
{
  class Class1
  {
  }

  class Class1<T1, T2>
  {
  }
}
", "").GetClassDeclarationSignatures();

    dictionary["Namespace1.Namespace2.Class1"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1"].Name.Should().Be("Class1");
    dictionary["Namespace1.Namespace2.Class1<T1,T2>"].Namespace.Should().Be("Namespace1.Namespace2");
    dictionary["Namespace1.Namespace2.Class1<T1,T2>"].Name.Should().Be("Class1<T1,T2>");
  }
    
}
