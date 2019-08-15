using System.Collections.Generic;
using AtmaFileSystem;
using NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode;

namespace TddXt.NScan.ReadingCSharpSourceCode
{
  public interface ICSharpFileSyntaxTree
  {
    AbsoluteFilePath FilePath { get; }
    IEnumerable<string> GetAllUniqueNamespaces();
    IReadOnlyList<string> GetAllUsingsFrom(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos);
    IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures();
  }
}
