using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TddXt.NScan.CSharp
{
  public class NamespaceGatheringVisitor : CSharpSyntaxVisitor
  {
    private readonly ISet<string> _resultSet = new HashSet<string>();

    public override void VisitCompilationUnit(CompilationUnitSyntax node)
    {
      foreach (var memberDeclarationSyntax in node.Members)
      {
        memberDeclarationSyntax.Accept(this);
      }
    }

    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
      _resultSet.Add(node.Name.ToString());
      foreach (var member in node.Members)
      {
        member.Accept(this);
      }
    }

    public ISet<string> ToSet()
    {
      return _resultSet;
    }
  }
}