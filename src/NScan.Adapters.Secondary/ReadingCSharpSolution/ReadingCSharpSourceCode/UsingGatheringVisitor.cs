using System.Collections.Generic;
using Core.NullableReferenceTypesExtensions;
using LanguageExt;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

public class UsingGatheringVisitor(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos)
  : CSharpSyntaxVisitor
{
  private readonly List<string> _usingNames = new();

  public override void VisitUsingDirective(UsingDirectiveSyntax node)
  {
    var usingSubject = TypeFormatting.StripWhitespace(node.Name.OrThrow().ToString());
    if (node.StaticKeyword.Value == null)
    {
      _usingNames.Add(usingSubject);
    }
    else if(classDeclarationInfos.TryGetValue(usingSubject, out var classDeclaration))
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

  public Seq<string> ToSeq()
  {
    return _usingNames.ToSeq();
  }
}
