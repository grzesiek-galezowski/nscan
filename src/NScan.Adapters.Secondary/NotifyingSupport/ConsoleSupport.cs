using System;
using AtmaFileSystem;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.Secondary.NotifyingSupport
{
  public class ConsoleSupport : INScanSupport
  {
    public static ConsoleSupport CreateInstance()
    {
      return new ConsoleSupport(Console.WriteLine);
    }

    private readonly Action<object> _writeLine;

    public ConsoleSupport(Action<object> writeLine)
    {
      _writeLine = writeLine;
    }

    private const string DiscoveredRule = "Discovered rule: ";

    public void Report(Exception exceptionFromResolution)
    {
      _writeLine(exceptionFromResolution);
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
      AbsoluteFilePath projectFilePath)
    {
      _writeLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
    }

    public void Log(IndependentRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{IndependentRuleMetadata.Format(dto)}");
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasCorrectNamespacesRuleMetadata.Format(dto)}");
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasNoCircularUsingsRuleMetadata.Format(dto)}");
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasAttributesOnRuleMetadata.Format(dto)}");
    }

    public void Log(HasTargetFrameworkRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasTargetFrameworkRuleMetadata.Format(dto)}");
    }

    public void Log(NoUsingsRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasNoUsingsRuleMetadata.Format(dto)}");
    }

    public void Log(HasPropertyRuleComplementDto dto)
    {
      _writeLine($"{DiscoveredRule}{HasPropertyRuleMetadata.Format(dto)}");
    }
  }
}
