using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedReportFragmentsFormat
  {
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}