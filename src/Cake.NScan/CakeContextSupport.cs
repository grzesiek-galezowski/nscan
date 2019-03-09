using System;
using AtmaFileSystem;
using Cake.Core.Diagnostics;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules;
using TddXt.NScan.ReadingRules.Ports;

namespace Cake.NScan
{
  public class CakeContextSupport : INScanSupport
  {
    private readonly ICakeLog _contextLog;

    public CakeContextSupport(ICakeLog contextLog)
    {
      _contextLog = contextLog;
    }

    public void Report(Exception exceptionFromResolution)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Error, exceptionFromResolution.ToString());
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
      AbsoluteFilePath projectFilePath)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Warning,
        $"Invalid format - skipping {projectFilePath} because of {invalidOperationException}");
    }

    public void Log(IndependentRuleComplementDto independentRuleDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {independentRuleDto.DependingPattern.Description()} {independentRuleDto.RuleName} {independentRuleDto.DependencyType}:{independentRuleDto.DependencyPattern.Pattern}" + "");
    }

    public void Log(CorrectNamespacesRuleComplementDto correctNamespacesRuleDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {correctNamespacesRuleDto.ProjectAssemblyNamePattern.Description()} {correctNamespacesRuleDto.RuleName}");
    }

    public void Log(NoCircularUsingsRuleComplementDto noCircularUsingsRuleDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {noCircularUsingsRuleDto.ProjectAssemblyNamePattern.Description()} {noCircularUsingsRuleDto.RuleName}");
    }
  }
}
