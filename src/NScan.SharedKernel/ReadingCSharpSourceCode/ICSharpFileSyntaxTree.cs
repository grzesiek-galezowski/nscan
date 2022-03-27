using System.Collections.Generic;
using AtmaFileSystem;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public interface ICSharpFileSyntaxTree
{
  AbsoluteFilePath FilePath { get; }
  IEnumerable<string> GetAllUniqueNamespaces();
  IReadOnlyList<string> GetAllUsingsFrom(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos);
  IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures();
}