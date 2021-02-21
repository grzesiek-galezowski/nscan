using System;
using System.Reflection;
using NScan.Lib;
using NScan.SharedKernel.WritingProgramOutput.Ports;

namespace NScan.Adapters.Secondary.ReportingOfResults
{
  public class ConsoleOutput : INScanOutput
  {
    public static ConsoleOutput CreateInstance()
    {
      return new ConsoleOutput(Console.WriteLine);
    }

    private readonly Action<string> _writeLine;

    public ConsoleOutput(Action<string> writeLine)
    {
      _writeLine = writeLine;
    }

    public void WriteAnalysisReport(string analysisReport)
    {
      _writeLine(analysisReport);
    }

    public void WriteVersion(string coreVersion)
    {
      var runnerVersion = Versioning.VersionOf(Assembly.GetExecutingAssembly());
      _writeLine("NScan Console Runner v" + runnerVersion + " based on core library v" + coreVersion);
    }
  }
}
