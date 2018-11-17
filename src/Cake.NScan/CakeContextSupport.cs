using System;
using Cake.Core.Diagnostics;
using TddXt.NScan.App;
using TddXt.NScan.RuleInputData;

namespace Cake.NScan
{
  public class CakeContextSupport : INScanSupport
  {
    private readonly ICakeLog _contextLog;

    public CakeContextSupport(ICakeLog contextLog)
    {
      Console.WriteLine(Environment.NewLine);
      _contextLog = contextLog;
    }

    public void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Error, exceptionFromResolution.ToString());
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Warning,
        $"Invalid format - skipping {projectFilePath} because of {invalidOperationException}");
    }

    public void LogIndependentRule(RuleDto ruleDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {ruleDto.DependingPattern.Description()} {ruleDto.RuleName} {ruleDto.IndependentRuleComplement.DependencyType}:{ruleDto.IndependentRuleComplement.DependencyPattern.Pattern}" + "");
    }

    public void LogNamespacesRule(RuleDto ruleDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {ruleDto.DependingPattern.Description()} {ruleDto.RuleName}");
    }
  }
}
