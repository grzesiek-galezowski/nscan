using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.Xml;

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

    private XmlSourceCodeFileBuilder(string fileName, List<string> declaredNamespaces)
    {
      FileName = fileName;
      DeclaredNamespaces = declaredNamespaces;
    }

    public List<string> DeclaredNamespaces { get; }
    public string FileName { get; private set; }

    public XmlSourceCodeFile BuildWith(string parentProjectAssemblyName, string parentProjectRootNamespace)
    {
      return new XmlSourceCodeFile(FileName, DeclaredNamespaces, parentProjectRootNamespace, parentProjectAssemblyName, 
        new List<string>(/* bug */));
    }
  }
}