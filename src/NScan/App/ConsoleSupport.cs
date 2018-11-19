using System;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.App
{
  public interface INScanSupport
  {
    void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution);
    void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath);
    void LogIndependentRule(IndependentRuleComplementDto independentRuleComplementDto);
    void LogNamespacesRule(CorrectNamespacesRuleComplementDto dto);
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

    public void LogIndependentRule(IndependentRuleComplementDto ruleComplementDto)
    {
      Console.WriteLine($"Discovered rule: {ruleComplementDto.DependingPattern.Description()} {ruleComplementDto.RuleName} {ruleComplementDto.DependencyType}:{ruleComplementDto.DependencyPattern.Pattern}" + "");
    }

    public void LogNamespacesRule(CorrectNamespacesRuleComplementDto dto)
    {
      Console.WriteLine($"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }
  }
}