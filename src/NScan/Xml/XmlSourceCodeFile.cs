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

    public string ParentProjectAssemblyName { get; set; }
    public string ParentProjectRootNamespace { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
  }
}