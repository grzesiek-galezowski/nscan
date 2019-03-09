using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AtmaFileSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace TddXt.NScan.ReadingCSharpSourceCode
{
  public class CSharpFileSyntaxTree
  {
    public static CSharpFileSyntaxTree ParseText(string sourceCode, string path)
    {
      return new CSharpFileSyntaxTree(CSharpSyntaxTree.ParseText(sourceCode, path: path));
    }

    private readonly SyntaxTree _syntaxTree;

    public CSharpFileSyntaxTree(SyntaxTree syntaxTree)
    {
      _syntaxTree = syntaxTree;
    }

    public AbsoluteFilePath FilePath => AbsoluteFilePath(_syntaxTree.FilePath);

    public IEnumerable<string> GetAllUniqueNamespaces()
    {
      var gatheringVisitor = new NamespaceGatheringVisitor();
      _syntaxTree.GetCompilationUnitRoot().Accept(gatheringVisitor);
      return gatheringVisitor.ToSet();
    }

    public IReadOnlyList<string> GetAllUsingsFrom(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos)
    {
      var usingGatheringVisitor = new UsingGatheringVisitor(classDeclarationInfos);
      _syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
      return usingGatheringVisitor.ToList();
    }

    public IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures()
    {
      var usingGatheringVisitor = new ClassGatheringVisitor();
      _syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
      var classDeclarationsByFullName = usingGatheringVisitor.ToDictionary();
      return classDeclarationsByFullName;
    }

    public static Dictionary<string, ClassDeclarationInfo> GetClassDeclarationSignaturesFromFiles(IEnumerable<CSharpFileSyntaxTree> cSharpSyntaxs)
    {
      return cSharpSyntaxs.SelectMany(syntax => syntax.GetClassDeclarationSignatures())
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public static CSharpFileSyntaxTree ParseFile(AbsoluteFilePath path)
    {
      return CSharpFileSyntaxTree.ParseText(File.ReadAllText(path.ToString()), path.ToString());
    }
  }
}