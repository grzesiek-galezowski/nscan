using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Functional.Maybe.Just;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TddXt.NScan.ReadingCSharpSourceCode
{
  public class ClassGatheringVisitor : CSharpSyntaxVisitor
  {
    private readonly List<ClassDeclarationInfo> _classes = new List<ClassDeclarationInfo>();

    public override void VisitCompilationUnit(CompilationUnitSyntax node)
    {
      VisitChildrenOf(node);
    }

    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
      VisitChildrenOf(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
      var className = node.Identifier.ValueText;
      var currentNamespace = Maybe<string>.Nothing;
      var currentParent = (CSharpSyntaxNode)node.Parent;
      while (!(currentParent is CompilationUnitSyntax))
      {
        if (currentParent is NamespaceDeclarationSyntax enclosingNamespace)
        {
          currentNamespace = currentNamespace.Select(n => enclosingNamespace.Name + "." + n).OrElse(() => enclosingNamespace.Name.ToString()).Just();
        }
        else if (currentParent is ClassDeclarationSyntax enclosingClass)
        {
          className = enclosingClass.Identifier.ValueText + "." + className;
        }

        currentParent = (CSharpSyntaxNode)currentParent.Parent;
      }

      _classes.Add(
        new ClassDeclarationInfo(className, currentNamespace.OrElse(() => string.Empty)));
      VisitChildrenOf(node);
    }

    public Dictionary<string, ClassDeclarationInfo> ToDictionary()
    {
      return _classes.ToDictionary(info => info.FullName);
    }

    private void VisitChildrenOf(SyntaxNode node)
    {
      foreach (var child in node.ChildNodes().Cast<CSharpSyntaxNode>())
      {
        child.Accept(this);
      }
    }
  }
}