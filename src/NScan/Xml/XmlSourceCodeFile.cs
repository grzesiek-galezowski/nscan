using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.NScan.Xml
{
  public class XmlSourceCodeFile
  {
    public XmlSourceCodeFile(string fileName,
      List<string> declaredNamespaces,
      string parentProjectRootNamespace,
      string parentProjectAssemblyName, 
      IReadOnlyList<string> usings)
    {
      Name = fileName ?? throw new ArgumentNullException(nameof(fileName));
      DeclaredNamespaces = declaredNamespaces;
      if (!DeclaredNamespaces.Any())
      {
        throw new ArgumentException(nameof(declaredNamespaces));
      }
      ParentProjectRootNamespace = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
      ParentProjectAssemblyName = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
    }

    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; }
    public string Name { get; }
    public List<string> DeclaredNamespaces { get; }
  }
}