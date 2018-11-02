using System;

namespace TddXt.NScan.CompositionRoot
{
  public interface INScanOutput
  {
    void Write(string analysisReport);
  }

  public class ConsoleOutput : INScanOutput
  {
    public void Write(string analysisReport)
    {
      Console.WriteLine(analysisReport);
    }
  }
}