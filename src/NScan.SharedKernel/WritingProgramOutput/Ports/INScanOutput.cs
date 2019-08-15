namespace TddXt.NScan.WritingProgramOutput.Ports
{
  public interface INScanOutput
  {
    void WriteAnalysisReport(string analysisReport);
    void WriteVersion(string coreVersion);
  }
}