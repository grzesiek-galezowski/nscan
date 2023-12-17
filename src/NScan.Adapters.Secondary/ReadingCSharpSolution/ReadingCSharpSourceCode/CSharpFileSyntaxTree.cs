using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AtmaFileSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

public class CSharpFileSyntaxTree(SyntaxTree syntaxTree) : ICSharpFileSyntaxTree
{
  public static CSharpFileSyntaxTree ParseText(string sourceCode, string path)
  {
    return new CSharpFileSyntaxTree(CSharpSyntaxTree.ParseText(sourceCode, path: path));
  }

  public AbsoluteFilePath FilePath => AbsoluteFilePath(syntaxTree.FilePath);

  public IEnumerable<string> GetAllUniqueNamespaces()
  {
    var gatheringVisitor = new NamespaceGatheringVisitor();
    syntaxTree.GetCompilationUnitRoot().Accept(gatheringVisitor);
    return gatheringVisitor.ToSet();
  }

  public IReadOnlyList<string> GetAllUsingsFrom(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos)
  {
    var usingGatheringVisitor = new UsingGatheringVisitor(classDeclarationInfos);
    syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
    return usingGatheringVisitor.ToList();
  }

  public IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures()
  {
    var usingGatheringVisitor = new ClassGatheringVisitor();
    syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
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
    return ParseText(File.ReadAllText(path.ToString()), path.ToString());
  }
}
