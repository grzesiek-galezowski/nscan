using System;
using System.Collections.Generic;

namespace TddXt.NScan.Xml
{
  public class XmlSourceCodeFile
  {
    public XmlSourceCodeFile(string fileName,
      string declaredNamespace,
      string parentProjectRootNamespace,
      string parentProjectAssemblyName, IReadOnlyList<string> getAllUsingsFromFile)
    {
      Name = fileName ?? throw new ArgumentNullException(nameof(fileName));
      DeclaredNamespace = declaredNamespace ?? throw new ArgumentNullException(nameof(declaredNamespace));
      ParentProjectRootNamespace = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
      ParentProjectAssemblyName = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
    }

    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; }
    public string Name { get; }
    public string DeclaredNamespace { get; }
  }
}