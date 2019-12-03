using System;
using AtmaFileSystem;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel.NotifyingSupport.Ports
{
  public interface INScanSupport
  {
    void Report(Exception exceptionFromResolution);

    void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
      AbsoluteFilePath projectFilePath);

    void Log(IndependentRuleComplementDto dto);
    void Log(CorrectNamespacesRuleComplementDto dto);
    void Log(NoCircularUsingsRuleComplementDto dto);
    void Log(HasAttributesOnRuleComplementDto dto);
    void Log(HasTargetFrameworkRuleComplementDto dto);
    void Log(NoUsingsRuleComplementDto dto);
  }
}