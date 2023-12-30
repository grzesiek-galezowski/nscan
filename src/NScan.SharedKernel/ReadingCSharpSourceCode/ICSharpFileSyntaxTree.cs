using System.Collections.Generic;
using AtmaFileSystem;
using LanguageExt;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public interface ICSharpFileSyntaxTree
{
  AbsoluteFilePath FilePath { get; }
  Seq<string> GetAllUniqueNamespaces();
  Seq<string> GetAllUsingsFrom(IReadOnlyDictionary<string, ClassDeclarationInfo> classDeclarationInfos);
  IReadOnlyDictionary<string, ClassDeclarationInfo> GetClassDeclarationSignatures();
}
