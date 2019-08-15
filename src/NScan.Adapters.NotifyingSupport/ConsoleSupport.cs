using System;
using AtmaFileSystem;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.Ports;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Adapter.NotifyingSupport
{
  public class ConsoleSupport : INScanSupport
  {
    private const string DiscoveredRule = "Discovered rule: ";

    public void Report(Exception exceptionFromResolution)
    {
      Console.WriteLine(exceptionFromResolution);
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
      AbsoluteFilePath projectFilePath)
    {
      Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
    }

    public void Log(IndependentRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{RuleFormats.Format(dto)}");
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{RuleFormats.Format(dto)}");
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{RuleFormats.Format(dto)}");
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{RuleFormats.Format(dto)}");
    }

    public void Log(HasTargetFrameworkRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{RuleFormats.Format(dto)}");
    }
  }
}