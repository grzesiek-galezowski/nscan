using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TddXt.NScan.Xml
{
  public static class CSharpSyntax
  {
    public static IEnumerable<string> GetAllUniqueNamespacesFrom(string fileText)
    {
      var tree = CSharpSyntaxTree.ParseText(fileText);
      return new HashSet<string>(tree.GetCompilationUnitRoot().Members.Where(MemberIsNamespace())
        .Cast<NamespaceDeclarationSyntax>().Select(NamespaceName()));
    }

    private static Func<NamespaceDeclarationSyntax, string> NamespaceName()
    {
      return ns => ns.Name.ToString();
    }

    private static Func<MemberDeclarationSyntax, bool> MemberIsNamespace()
    {
      return m => m is NamespaceDeclarationSyntax;
    }

    public static IEnumerable<string> GetAllUniqueNamespacesFromFile(string file)
    {
      return CSharpSyntax.GetAllUniqueNamespacesFrom(File.ReadAllText(file));
    }
  }
}