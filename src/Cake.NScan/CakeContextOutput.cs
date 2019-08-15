using System.Reflection;
using Cake.Core.Diagnostics;
using NScan.Lib;
using NScan.SharedKernel.WritingProgramOutput.Ports;

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

    public void WriteVersion(string coreVersion)
    {
      var runnerVersion = Versioning.VersionOf(Assembly.GetExecutingAssembly());
      _contextLog.Write(Verbosity.Minimal, LogLevel.Information,
        $"NScan Cake Addin v{runnerVersion} based on core library v{coreVersion}");
    }
  }
}