using System;
using TddXt.NScan.WritingProgramOutput.Ports;

namespace TddXt.NScan.WritingProgramOutput.Adapters
{
  public class ConsoleOutput : INScanOutput
  {
    public void WriteAnalysisReport(string analysisReport)
    {
      Console.WriteLine(analysisReport);
    }
  }
}