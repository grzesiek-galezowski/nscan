using System;
using AtmaFileSystem;
using Cake.Core.Diagnostics;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

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
      //bug duplication
      Log($"{dto.ProjectAssemblyNamePattern.Description()} " +
                        $"hasNoUsings from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}");
    }

    private void Log(string ruleDescription)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, Discovered(ruleDescription));
    }

    private static string Discovered(string ruleDescription)
    {
      return $"Discovered rule: {ruleDescription}";
    }
  }


}
