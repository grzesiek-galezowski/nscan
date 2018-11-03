using System;

namespace TddXt.NScan.CompositionRoot
{
  public interface INScanOutput
  {
    void WriteAnalysisReport(string analysisReport);
  }

  public class ConsoleOutput : INScanOutput
  {
    public void WriteAnalysisReport(string analysisReport)
    {
      Console.WriteLine(analysisReport);
    }
  }
}