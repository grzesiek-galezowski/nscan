using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingSolution.Ports;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class XmlSourceCodeFileBuilder
  {
    private readonly List<ClassDeclarationBuilder> _classes = new List<ClassDeclarationBuilder>();
    private List<string> DeclaredNamespaces { get; }
    private string FileName { get; set; }
    private List<string> Usings { get; } = new List<string>();

    public static XmlSourceCodeFileBuilder FileWithNamespace(string fileName, string fileNamespace)
    {
      return new XmlSourceCodeFileBuilder(fileName, new List<string> { fileNamespace });
    }

    public static XmlSourceCodeFileBuilder EmptyFile(string fileName)
    {
      return new XmlSourceCodeFileBuilder(fileName, Enumerable.Empty<string>().ToList());
    }

    public static XmlSourceCodeFileBuilder FileWithNamespaces(string fileName, params string[] namespaces)
    {
      return new XmlSourceCodeFileBuilder(fileName, namespaces.ToList());
    }

    private XmlSourceCodeFileBuilder(string fileName, List<string> declaredNamespaces)
    {
      FileName = fileName;
      DeclaredNamespaces = declaredNamespaces;
    }

    public XmlSourceCodeFileBuilder Using(string usingDeclaration)
    {
      Usings.Add(usingDeclaration);
      return this;
    }

    public XmlSourceCodeFile BuildWith(string parentProjectAssemblyName, string parentProjectRootNamespace)
    {
      return new XmlSourceCodeFile(
        AtmaFileSystemPaths.RelativeFilePath(FileName), 
        DeclaredNamespaces, 
        parentProjectRootNamespace, 
        parentProjectAssemblyName, 
        Usings,
        _classes.Select(c => c.Build()).ToList());
    }

    public static XmlSourceCodeFileBuilder File(string fileName)
    {
      return FileWithNamespace(fileName, Any.AlphaString());
    }


    public XmlSourceCodeFileBuilder With(ClassDeclarationBuilder classDeclarationBuilder)
    {
      _classes.Add(classDeclarationBuilder);
      return this;
    }
  }
}