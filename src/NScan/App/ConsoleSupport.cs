using System;
using TddXt.NScan.CompositionRoot;

namespace TddXt.NScan.App
{
  public interface INScanSupport
  {
    void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution);
    void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath);
    void LogRule(RuleDto ruleDto);
  }

  public class ConsoleSupport : INScanSupport
  {
    public void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution)
    {
      Console.WriteLine(exceptionFromResolution);
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath)
    {
      Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
    }

    public void LogRule(RuleDto ruleDto)
    {
      Console.WriteLine($"Discovered rule: {ruleDto.DependingPattern.Description()} {ruleDto.RuleName} {ruleDto.IndependentRuleComplement.DependencyType}:{ruleDto.IndependentRuleComplement.DependencyPattern.Pattern}" + "");
    }
  }
}