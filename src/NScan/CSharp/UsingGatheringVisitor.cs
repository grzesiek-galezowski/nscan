using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TddXt.NScan.CSharp
{
  public class UsingGatheringVisitor : CSharpSyntaxVisitor
  {
    private readonly IReadOnlyDictionary<string, ClassDeclarationInfo> _classDeclarationInfos;
    private readonly List<string> _usingNames = new List<string>();

    public UsingGatheringVisitor(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos)
    {
      _classDeclarationInfos = classDeclarationInfos;
    }

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
      var usingSubject = node.Name.ToString();
      if (node.StaticKeyword.Value == null)
      {
        _usingNames.Add(usingSubject);
      }
      else if(_classDeclarationInfos.TryGetValue(usingSubject, out var classDeclaration))
      {
        _usingNames.Add(classDeclaration.Namespace);
      }
    }

    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
      foreach(var u in node.Usings) {u.Accept(this);}
      foreach(var u in node.Members) {u.Accept(this);}
    }

    public override void VisitCompilationUnit(CompilationUnitSyntax node)
    {
      foreach(var u in node.Usings) {u.Accept(this);}
      foreach(var u in node.Members) {u.Accept(this);}
    }

    public IReadOnlyList<string> ToList()
    {
      return _usingNames;
    }
  }
}