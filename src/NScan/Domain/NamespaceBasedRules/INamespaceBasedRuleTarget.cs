﻿using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface INamespaceBasedRuleTarget
  {
    void RefreshNamespacesCache();
    void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report);
  }
}