using System;
using AtmaFileSystem;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.NotifyingSupport.Adapters
{
  public class ConsoleSupport : INScanSupport
  {
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
      Console.WriteLine($"Discovered rule: {dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}" + "");
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
      Console.WriteLine($"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
      Console.WriteLine($"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
      throw new NotImplementedException();
    }
  }
}