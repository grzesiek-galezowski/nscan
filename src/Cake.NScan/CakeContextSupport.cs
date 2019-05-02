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

    public void Log(IndependentRuleComplementDto dto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}" + "");
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
      //bug there's duplication in the output of these rules
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} {dto.ClassNameInclusionPattern}:{dto.MethodNameInclusionPattern}");
    }
  }
}
