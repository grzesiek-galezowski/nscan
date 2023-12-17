using System;
using AtmaFileSystem;
using Cake.Core.Diagnostics;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace Cake.NScan;

public class CakeContextSupport(ICakeLog contextLog) : INScanSupport
{
  public void Report(Exception exceptionFromResolution)
  {
    contextLog.Write(Verbosity.Minimal, LogLevel.Error, exceptionFromResolution.ToString());
  }

  public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
    AbsoluteFilePath projectFilePath)
  {
    contextLog.Write(Verbosity.Minimal, LogLevel.Warning,
      $"Invalid format - skipping {projectFilePath} because of {invalidOperationException}");
  }

  public void Log(IndependentRuleComplementDto dto)
  {
    Log(IndependentRuleMetadata.Format(dto));
  }

  public void Log(CorrectNamespacesRuleComplementDto dto)
  {
    Log(HasCorrectNamespacesRuleMetadata.Format(dto));
  }

  public void Log(NoCircularUsingsRuleComplementDto dto)
  {
    Log(HasNoCircularUsingsRuleMetadata.Format(dto));
  }

  public void Log(HasAttributesOnRuleComplementDto dto)
  {
    Log(HasAttributesOnRuleMetadata.Format(dto));
  }

  public void Log(HasTargetFrameworkRuleComplementDto dto)
  {
    Log(HasTargetFrameworkRuleMetadata.Format(dto));
  }

  public void Log(NoUsingsRuleComplementDto dto)
  {
    Log(HasNoUsingsRuleMetadata.Format(dto));
  }

  public void Log(HasPropertyRuleComplementDto dto)
  {
    Log(HasPropertyRuleMetadata.Format(dto));
  }

  private void Log(RuleDescription ruleDescription)
  {
    contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, Discovered(ruleDescription));
  }

  private static string Discovered(RuleDescription ruleDescription)
  {
    return $"Discovered rule: {ruleDescription}";
  }
}
