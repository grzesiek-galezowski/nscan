using System;
using System.Reflection;
using NScan.Lib;
using NScan.SharedKernel.WritingProgramOutput.Ports;

namespace NScan.Adapter.ReportingOfResults
{
  public class ConsoleOutput : INScanOutput
  {
    public void WriteAnalysisReport(string analysisReport)
    {
      Console.WriteLine(analysisReport);
    }

    public void WriteVersion(string coreVersion)
    {
      var runnerVersion = Versioning.VersionOf(Assembly.GetExecutingAssembly());
      Console.WriteLine("NScan Console Runner v" + runnerVersion + " based on core library v" + coreVersion);
    }
  }
}