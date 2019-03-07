using System;
using System.Collections.Generic;

namespace TddXt.NScan.ReadingSolution.Ports
{
  public class XmlSourceCodeFile
  {
    public XmlSourceCodeFile(string fileName,
      List<string> declaredNamespaces,
      string parentProjectRootNamespace,
      string parentProjectAssemblyName, 
      IReadOnlyList<string> usings)
    {
      Usings = usings ?? throw new ArgumentException(nameof(usings));
      Name = fileName ?? throw new ArgumentNullException(nameof(fileName));
      DeclaredNamespaces = declaredNamespaces ?? throw new ArgumentException(nameof(declaredNamespaces));
      ParentProjectRootNamespace = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
      ParentProjectAssemblyName = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
    }

    public IReadOnlyList<string> Usings { get; }
    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; }
    public string Name { get; } //bug FileName
    public IReadOnlyList<string> DeclaredNamespaces { get; }
  }
}