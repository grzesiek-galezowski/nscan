namespace TddXt.NScan.Xml
{
  public class XmlSourceCodeFile
  {
    public XmlSourceCodeFile(
      string fileName, 
      string @namespace, 
      string parentProjectRootNamespace, 
      string parentProjectAssemblyName)
    {
      Name = fileName;
      Namespace = @namespace;
      ParentProjectRootNamespace = parentProjectRootNamespace;
      ParentProjectAssemblyName = parentProjectAssemblyName;
    }

    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; set; } //bug introduce builder
    public string Name { get; }
    public string Namespace { get; set; } //bug introduce builder
  }
}