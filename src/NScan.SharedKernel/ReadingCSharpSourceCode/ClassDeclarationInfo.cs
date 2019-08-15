using System.Collections.Generic;

namespace NScan.SharedKernel.ReadingCSharpSourceCode
{
  public class ClassDeclarationInfo
  {
    public ClassDeclarationInfo(string className, string @namespace)
    {
      Name = className;
      Namespace = @namespace;
    }

    public string FullName => NamespacePrefix() + Name;
    public string Namespace { get; }
    public string Name { get; }
    public List<MethodDeclarationInfo> Methods { get; } = new List<MethodDeclarationInfo>();

    private string NamespacePrefix()
    {
      return Namespace != string.Empty ? Namespace + "." : "";
    }
  }
}