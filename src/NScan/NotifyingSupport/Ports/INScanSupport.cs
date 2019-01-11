using System;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.NotifyingSupport.Ports
{
  public interface INScanSupport
  {
    void Report(Exception exceptionFromResolution);
    void SkippingProjectBecauseOfError(
      InvalidOperationException invalidOperationException, string projectFilePath);
    void Log(IndependentRuleComplementDto independentRuleDto);
    void Log(CorrectNamespacesRuleComplementDto correctNamespacesRuleDto);
    void Log(NoCircularUsingsRuleComplementDto noCircularUsingsRuleDto);
  }
}