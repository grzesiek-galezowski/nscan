using System;
using AtmaFileSystem;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.NotifyingSupport.Ports
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
  }
}