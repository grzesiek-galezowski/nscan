using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using LanguageExt;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScanSpecification.Lib.AutomationLayer;

public class SourceCodeFileDtoBuilder
{
  private Seq<ClassDeclarationBuilder> _classes;
  private Seq<string> DeclaredNamespaces { get; }
  private string FileName { get; set; }
  private Seq<string> Usings { get; set; }

  public static SourceCodeFileDtoBuilder FileWithNamespace(string fileName, string fileNamespace)
  {
    return new SourceCodeFileDtoBuilder(fileName, [fileNamespace]);
  }

  public static SourceCodeFileDtoBuilder EmptyFile(string fileName)
  {
    return new SourceCodeFileDtoBuilder(fileName, Enumerable.Empty<string>().ToList());
  }

  public static SourceCodeFileDtoBuilder FileWithNamespaces(string fileName, params string[] namespaces)
  {
    return new SourceCodeFileDtoBuilder(fileName, namespaces.ToList());
  }

  private SourceCodeFileDtoBuilder(string fileName, List<string> declaredNamespaces)
  {
    FileName = fileName;
    DeclaredNamespaces = declaredNamespaces.ToSeq(); //bug
  }

  public SourceCodeFileDtoBuilder Using(string usingDeclaration)
  {
    Usings = Usings.Add(usingDeclaration);
    return this;
  }

  public SourceCodeFileDto BuildWith(string parentProjectAssemblyName, string parentProjectRootNamespace)
  {
    return new SourceCodeFileDto(
      AtmaFileSystemPaths.RelativeFilePath(FileName), 
      DeclaredNamespaces, 
      parentProjectRootNamespace, 
      parentProjectAssemblyName, 
      Usings.ToSeq(),
      _classes.Select(c => c.WithNamespace(DeclaredNamespaces.First()).Build()));
  }

  public static SourceCodeFileDtoBuilder File(string fileName)
  {
    return FileWithNamespace(fileName, Any.AlphaString());
  }


  public SourceCodeFileDtoBuilder Containing(ClassDeclarationBuilder classDeclarationBuilder)
  {
    _classes = _classes.Add(classDeclarationBuilder);
    return this;
  }
}
