using System;
using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.App
{
  public interface INScanSupport
  {
    void Report(Exception exceptionFromResolution);
    void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath);
    void Log(IndependentRuleComplementDto independentRuleDto);
    void Log(CorrectNamespacesRuleComplementDto correctNamespacesRuleDto);
    void Log(NoCircularUsingsRuleComplementDto noCircularUsingsRuleDto);
  }

  public class ConsoleSupport : INScanSupport
  {
    public void Report(Exception exceptionFromResolution)
    {
      Console.WriteLine(exceptionFromResolution);
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath)
    {
      Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
    }

    public void Log(IndependentRuleComplementDto independentRuleDto)
    {
      Console.WriteLine($"Discovered rule: {independentRuleDto.DependingPattern.Description()} {independentRuleDto.RuleName} {independentRuleDto.DependencyType}:{independentRuleDto.DependencyPattern.Pattern}" + "");
    }

    public void Log(CorrectNamespacesRuleComplementDto correctNamespacesRuleDto)
    {
      Console.WriteLine($"Discovered rule: {correctNamespacesRuleDto.ProjectAssemblyNamePattern.Description()} {correctNamespacesRuleDto.RuleName}");
    }

    public void Log(NoCircularUsingsRuleComplementDto noCircularUsingsRuleDto)
    {
      Console.WriteLine($"Discovered rule: {noCircularUsingsRuleDto.ProjectAssemblyNamePattern.Description()} {noCircularUsingsRuleDto.RuleName}");
    }
  }
}