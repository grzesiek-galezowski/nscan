namespace NScan.SharedKernel.WritingProgramOutput.Ports
{
  public interface INScanOutput
  {
    void WriteAnalysisReport(string analysisReport);
    void WriteVersion(string coreVersion);
  }
}