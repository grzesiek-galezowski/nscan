using TddXt.NScan.Xml;

namespace TddXt.NScan.Domain
{
  public class SourceCodeFile : ISourceCodeFile
  {
    private readonly XmlSourceCodeFile _xmlSourceCodeFile;

    public SourceCodeFile(XmlSourceCodeFile xmlSourceCodeFile)
    {
      _xmlSourceCodeFile = xmlSourceCodeFile;
    }

    public void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription)
    {
      if (_xmlSourceCodeFile.ParentProjectRootNamespace != _xmlSourceCodeFile.Namespace)
      {
        report.ProjectScopedViolation(ruleDescription,
          _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
          _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
          + _xmlSourceCodeFile.Name + " located in project root folder has incorrect namespace "
          + _xmlSourceCodeFile.Namespace
        );
      }
    }
  }
}