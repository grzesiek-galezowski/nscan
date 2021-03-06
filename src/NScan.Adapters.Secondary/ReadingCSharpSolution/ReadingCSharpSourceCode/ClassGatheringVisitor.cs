﻿using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Functional.Maybe.Just;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NullableReferenceTypesExtensions;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode
{
  public class ClassGatheringVisitor : CSharpSyntaxVisitor
  {
    private readonly List<ClassDeclarationInfo> _classes = new();

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
      var className = node.Identifier.ValueText + GenericParameters(node);
      var currentNamespace = Maybe<string>.Nothing;
      var currentParent = (CSharpSyntaxNode)node.Parent.OrThrow();
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

        currentParent = (CSharpSyntaxNode)currentParent.Parent.OrThrow();
      }

      _classes.Add(
        new ClassDeclarationInfo(className, currentNamespace.OrElse(() => string.Empty) ));
      VisitChildrenOf(node);
    }

    private string GenericParameters(ClassDeclarationSyntax node)
    {
      if (node.TypeParameterList == null)
      {
        return string.Empty;
      }
      else
      {
        return $"<{GenericTypeListWithRemovedWhitespaces(node.TypeParameterList)}>";
      }
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

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
      var attributes = new List<string>();
      
      foreach (var attributeListSyntax in node.AttributeLists)
      {
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
          attributes.Add(attributeSyntax.Name.ToFullString());
        }
      }

      _classes.Last().Methods.Add(new MethodDeclarationInfo(node.Identifier.Value.OrThrow().ToString(), attributes));
    }
    
    private static string GenericTypeListWithRemovedWhitespaces(TypeParameterListSyntax typeParameterList)
    {
      return TypeFormatting.StripWhitespace(typeParameterList.Parameters.ToFullString());
    }
  }
}
