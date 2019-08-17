using System;
using AtmaFileSystem;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos;

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
      Console.WriteLine($"{DiscoveredRule}{IndependentRuleMetadata.Format(dto)}");
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{HasCorrectNamespacesRuleMetadata.Format(dto)}");
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{HasNoCircularUsingsRuleMetadata.Format(dto)}");
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{HasAttributesOnRuleMetadata.Format(dto)}");
    }

    public void Log(HasTargetFrameworkRuleComplementDto dto)
    {
      Console.WriteLine($"{DiscoveredRule}{HasTargetFrameworkRuleMetadata.Format(dto)}");
    }
  }
}