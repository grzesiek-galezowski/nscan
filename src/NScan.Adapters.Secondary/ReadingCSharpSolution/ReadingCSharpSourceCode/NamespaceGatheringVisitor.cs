using LanguageExt;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

public class NamespaceGatheringVisitor : CSharpSyntaxVisitor
{
  private Set<string> _resultSet;

  public override void VisitCompilationUnit(CompilationUnitSyntax node)
  {
    foreach (var memberDeclarationSyntax in node.Members)
    {
      memberDeclarationSyntax.Accept(this);
    }
  }

  public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
  {
    _resultSet = _resultSet.TryAdd(node.Name.ToString());
    foreach (var member in node.Members)
    {
      member.Accept(this);
    }
  }

  public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
  {
    _resultSet = _resultSet.TryAdd(node.Name.ToString());
    foreach (var member in node.Members)
    {
      member.Accept(this);
    }
  }

  public Set<string> ToSet()
  {
    return _resultSet;
  }
}
