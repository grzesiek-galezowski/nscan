using TddXt.NScan.Xml;

namespace TddXt.NScan.Domain
{
  public class SourceCodeFile : ISourceCodeFile
  {
    public SourceCodeFile(XmlSourceCodeFile xmlSourceCodeFile)
    {

    }

    public void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }
  }
}