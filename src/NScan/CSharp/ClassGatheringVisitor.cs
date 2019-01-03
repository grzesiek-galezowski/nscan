using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TddXt.NScan.Lib;

namespace TddXt.NScan.CSharp
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
      var currentNamespace = Maybe.Nothing<string>();
      var currentParent = (CSharpSyntaxNode)node.Parent;
      while (!(currentParent is CompilationUnitSyntax))
      {
        if (currentParent is NamespaceDeclarationSyntax enclosingNamespace)
        {
          currentNamespace = Maybe.Just(currentNamespace.Select(n => enclosingNamespace.Name + "." + n).ValueOr(enclosingNamespace.Name.ToString()));
        }
        else if (currentParent is ClassDeclarationSyntax enclosingClass)
        {
          className = enclosingClass.Identifier.ValueText + "." + className;
        }

        currentParent = (CSharpSyntaxNode)currentParent.Parent;
      }

      Console.WriteLine("Found " + className + " in namespace " + currentNamespace.ValueOr(string.Empty));
      _classes.Add(new ClassDeclarationInfo(className, currentNamespace.ValueOr(string.Empty)));
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