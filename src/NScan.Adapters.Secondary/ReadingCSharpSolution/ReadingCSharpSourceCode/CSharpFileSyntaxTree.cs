using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AtmaFileSystem;
using LanguageExt;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;

public class CSharpFileSyntaxTree(SyntaxTree syntaxTree) :  ICSharpFileSyntaxTree
{
  public static CSharpFileSyntaxTree ParseText(string sourceCode, string path)
  {
    return new CSharpFileSyntaxTree(CSharpSyntaxTree.ParseText(sourceCode, path: path));
  }

  public AbsoluteFilePath FilePath => AbsoluteFilePath(syntaxTree.FilePath);

  public Seq<string> GetAllUniqueNamespaces()
  {
    var gatheringVisitor = new NamespaceGatheringVisitor();
    syntaxTree.GetCompilationUnitRoot().Accept(gatheringVisitor);
    return gatheringVisitor.ToSet().ToSeq();
  }

  public Seq<string> GetAllUsingsFrom(Map<string, ClassDeclarationInfo> classDeclarationInfos)
  {
    var usingGatheringVisitor = new UsingGatheringVisitor(classDeclarationInfos);
    syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
    return usingGatheringVisitor.ToSeq();
  }

  public Map<string, ClassDeclarationInfo> GetClassDeclarationSignatures()
  {
    var usingGatheringVisitor = new ClassGatheringVisitor();
    syntaxTree.GetCompilationUnitRoot(CancellationToken.None).Accept(usingGatheringVisitor);
    var classDeclarationsByFullName = usingGatheringVisitor.ToMap();
    return classDeclarationsByFullName;
  }

  public static Map<string, ClassDeclarationInfo> GetClassDeclarationSignaturesFromFiles(IEnumerable<CSharpFileSyntaxTree> cSharpSyntaxs)
  {
    return cSharpSyntaxs.SelectMany(syntax => syntax.GetClassDeclarationSignatures())
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value).ToMap();
  }

  public static CSharpFileSyntaxTree ParseFile(AbsoluteFilePath path)
  {
    return ParseText(File.ReadAllText(path.ToString()), path.ToString());
  }
}
