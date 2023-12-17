using System.Collections.Generic;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public class ClassDeclarationInfo(string className, string @namespace)
{
  public string FullName => NamespacePrefix() + Name;
  public string Namespace { get; } = @namespace;
  public string Name { get; } = className;
  public List<MethodDeclarationInfo> Methods { get; } = new();

  private string NamespacePrefix()
  {
    return Namespace != string.Empty ? Namespace + "." : "";
  }
}
