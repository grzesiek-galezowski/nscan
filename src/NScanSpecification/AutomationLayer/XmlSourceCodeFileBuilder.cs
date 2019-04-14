using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class XmlSourceCodeFileBuilder
  {

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

    private List<string> DeclaredNamespaces { get; }
    private string FileName { get; set; }
    private List<string> Usings { get; } = new List<string>();

    public XmlSourceCodeFileBuilder Using(string usingDeclaration)
    {
      Usings.Add(usingDeclaration);
      return this;
    }

    public XmlSourceCodeFile BuildWith(string parentProjectAssemblyName, string parentProjectRootNamespace)
    {
      string fileName = FileName;
      return new XmlSourceCodeFile(
        AtmaFileSystemPaths.RelativeFilePath(fileName), 
        DeclaredNamespaces, 
        parentProjectRootNamespace, 
        parentProjectAssemblyName, 
        Usings);
    }

    public static XmlSourceCodeFileBuilder File(string fileName)
    {
      return EmptyFile(fileName);
    }


    public XmlSourceCodeFileBuilder With(XmlClassBuilder xmlClassBuilder)
    {
      return this;
    }
  }
}