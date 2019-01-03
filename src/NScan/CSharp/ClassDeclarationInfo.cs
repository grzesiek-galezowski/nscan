namespace TddXt.NScan.CSharp
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

    private string NamespacePrefix()
    {
      return Namespace != string.Empty ? Namespace + "." : "";
    }
  }
}