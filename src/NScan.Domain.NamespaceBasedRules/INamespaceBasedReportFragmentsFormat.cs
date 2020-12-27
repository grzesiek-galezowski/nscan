using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedReportFragmentsFormat
  {
    string ApplyTo(IReadOnlyList<IReadOnlyList<NamespaceName>> paths, string header);
  }
}
