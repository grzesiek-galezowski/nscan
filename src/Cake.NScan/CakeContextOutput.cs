using Cake.Core.Diagnostics;
using TddXt.NScan.WritingProgramOutput.Ports;

namespace Cake.NScan
{
  public class CakeContextOutput : INScanOutput
  {
    private readonly ICakeLog _contextLog;

    public CakeContextOutput(ICakeLog contextLog)
    {
      _contextLog = contextLog;
    }

    public void WriteAnalysisReport(string analysisReport)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Information, analysisReport);
    }
  }
}