﻿using Core.NullableReferenceTypesExtensions;
using LanguageExt;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

public class UsingGatheringVisitor(HashMap<string, ClassDeclarationInfo> classDeclarationInfos)
  : CSharpSyntaxVisitor
{
  private Seq<string> _usingNames;

  public override void VisitUsingDirective(UsingDirectiveSyntax node)
  {
    var usingSubject = TypeFormatting.StripWhitespace(node.Name.OrThrow().ToString());
    if (node.StaticKeyword.Value == null)
    {
      _usingNames = _usingNames.Add(usingSubject);
    }
    else
    {
      var classDeclaration = classDeclarationInfos.Find(usingSubject);
      classDeclaration.IfSome(info =>
      {
        _usingNames = _usingNames.Add(info.Namespace);
      });
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

  public Seq<string> ToSeq()
  {
    return _usingNames;
  }
}
