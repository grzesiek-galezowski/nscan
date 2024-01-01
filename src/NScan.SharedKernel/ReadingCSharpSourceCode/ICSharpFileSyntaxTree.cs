using AtmaFileSystem;
using LanguageExt;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public interface ICSharpFileSyntaxTree
{
  AbsoluteFilePath FilePath { get; }
  Seq<string> GetAllUniqueNamespaces();
  Seq<string> GetAllUsingsFrom(HashMap<string, ClassDeclarationInfo> classDeclarationInfos);
  HashMap<string, ClassDeclarationInfo> GetClassDeclarationSignatures();
}
