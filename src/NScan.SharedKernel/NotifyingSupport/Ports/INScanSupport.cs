using System;
using AtmaFileSystem;
using NScan.SharedKernel.Ports;

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
  }
}