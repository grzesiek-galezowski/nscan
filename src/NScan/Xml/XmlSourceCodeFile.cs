namespace TddXt.NScan.Xml
{
  public class XmlSourceCodeFile
  {
    public XmlSourceCodeFile(
      string fileName, 
      string declaredNamespace, 
      string parentProjectRootNamespace, 
      string parentProjectAssemblyName)
    {
      Name = fileName;
      DeclaredNamespace = declaredNamespace;
      ParentProjectRootNamespace = parentProjectRootNamespace;
      ParentProjectAssemblyName = parentProjectAssemblyName;
    }

    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; set; } //bug introduce builder
    public string Name { get; set; }
    public string DeclaredNamespace { get; set; } //bug introduce builder
  }
}