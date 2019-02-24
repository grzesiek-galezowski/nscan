using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.ReadingSolution.Ports;

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
      return new XmlSourceCodeFile(
        FileName, 
        DeclaredNamespaces, 
        parentProjectRootNamespace, 
        parentProjectAssemblyName, 
        Usings);
    }

  }
}