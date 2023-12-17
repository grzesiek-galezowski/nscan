using System;
using System.Reflection;
using NScan.Lib;
using NScan.SharedKernel.WritingProgramOutput.Ports;

namespace NScan.Adapters.Secondary.ReportingOfResults;

public class ConsoleOutput(Action<string> writeLine) : INScanOutput
{
  public static ConsoleOutput CreateInstance()
  {
    return new ConsoleOutput(Console.WriteLine);
  }

  public void WriteAnalysisReport(string analysisReport)
  {
    writeLine(analysisReport);
  }

  public void WriteVersion(string coreVersion)
  {
    var runnerVersion = Versioning.VersionOf(Assembly.GetExecutingAssembly());
    writeLine("NScan Console Runner v" + runnerVersion + " based on core library v" + coreVersion);
  }
}
