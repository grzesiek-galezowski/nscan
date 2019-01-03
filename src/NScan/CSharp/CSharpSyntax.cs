using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;

namespace TddXt.NScan.CSharp
{
  public static class CSharpSyntax
  {
    public static IEnumerable<string> GetAllUniqueNamespacesFrom(string fileText)
    {
      var tree = CSharpSyntaxTree.ParseText(fileText);
      var gatheringVisitor = new NamespaceGatheringVisitor();
      tree.GetCompilationUnitRoot().Accept(gatheringVisitor);
      return gatheringVisitor.ToSet();
    }

    public static IEnumerable<string> GetAllUniqueNamespacesFromFile(string file)
    {
      return GetAllUniqueNamespacesFrom(File.ReadAllText(file));
    }

    public static IReadOnlyList<string> GetAllUsingsFrom(string text, IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos)
    {
      var syntaxTree = CSharpSyntaxTree.ParseText(text);
      var usingGatheringVisitor = new UsingGatheringVisitor(classDeclarationInfos);
      syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
      return usingGatheringVisitor.ToList();
    }

    public static IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures(string sourceCode)
    {
      var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
      var usingGatheringVisitor = new ClassGatheringVisitor();
      syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
      var classDeclarationsByFullName = usingGatheringVisitor.ToDictionary();
      return classDeclarationsByFullName;
    }

    public static Dictionary<string, ClassDeclarationInfo> GetClassDeclarationSignaturesFromFiles(IEnumerable<string> sourceCodeFilesInProject)
    {
      return sourceCodeFilesInProject.SelectMany(
          path => CSharpSyntax.GetClassDeclarationSignatures(
            File.ReadAllText(path)))
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public static IReadOnlyList<string> GetAllUsingsFromFile(string file, Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures)
    {
      return CSharpSyntax.GetAllUsingsFrom(File.ReadAllText(file), classDeclarationSignatures);
    }
  }
}