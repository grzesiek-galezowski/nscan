using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using NScan.SharedKernel.ReadingSolution.Ports;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Lib.AutomationLayer;

public class SourceCodeFileDtoBuilder
{
  private readonly List<ClassDeclarationBuilder> _classes = new();
  private List<string> DeclaredNamespaces { get; }
  private string FileName { get; set; }
  private List<string> Usings { get; } = new();

  public static SourceCodeFileDtoBuilder FileWithNamespace(string fileName, string fileNamespace)
  {
    return new SourceCodeFileDtoBuilder(fileName, new List<string> { fileNamespace });
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
    DeclaredNamespaces = declaredNamespaces;
  }

  public SourceCodeFileDtoBuilder Using(string usingDeclaration)
  {
    Usings.Add(usingDeclaration);
    return this;
  }

  public SourceCodeFileDto BuildWith(string parentProjectAssemblyName, string parentProjectRootNamespace)
  {
    return new SourceCodeFileDto(
      AtmaFileSystemPaths.RelativeFilePath(FileName), 
      DeclaredNamespaces, 
      parentProjectRootNamespace, 
      parentProjectAssemblyName, 
      Usings,
      _classes.Select(c => c.WithNamespace(DeclaredNamespaces.First()).Build()).ToList());
  }

  public static SourceCodeFileDtoBuilder File(string fileName)
  {
    return FileWithNamespace(fileName, Any.AlphaString());
  }


  public SourceCodeFileDtoBuilder Containing(ClassDeclarationBuilder classDeclarationBuilder)
  {
    _classes.Add(classDeclarationBuilder);
    return this;
  }
}