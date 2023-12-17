using System;
using AtmaFileSystem;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.Secondary.NotifyingSupport;

public class ConsoleSupport(Action<object> writeLine) : INScanSupport
{
  public static ConsoleSupport CreateInstance()
  {
    return new ConsoleSupport(Console.WriteLine);
  }

  private const string DiscoveredRule = "Discovered rule: ";

  public void Report(Exception exceptionFromResolution)
  {
    writeLine(exceptionFromResolution);
  }

  public void SkippingProjectBecauseOfError(
    InvalidOperationException invalidOperationException,
    AbsoluteFilePath projectFilePath)
  {
    writeLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
  }

  public void Log(IndependentRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{IndependentRuleMetadata.Format(dto)}");
  }

  public void Log(CorrectNamespacesRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasCorrectNamespacesRuleMetadata.Format(dto)}");
  }

  public void Log(NoCircularUsingsRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasNoCircularUsingsRuleMetadata.Format(dto)}");
  }

  public void Log(HasAttributesOnRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasAttributesOnRuleMetadata.Format(dto)}");
  }

  public void Log(HasTargetFrameworkRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasTargetFrameworkRuleMetadata.Format(dto)}");
  }

  public void Log(NoUsingsRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasNoUsingsRuleMetadata.Format(dto)}");
  }

  public void Log(HasPropertyRuleComplementDto dto)
  {
    writeLine($"{DiscoveredRule}{HasPropertyRuleMetadata.Format(dto)}");
  }
}
