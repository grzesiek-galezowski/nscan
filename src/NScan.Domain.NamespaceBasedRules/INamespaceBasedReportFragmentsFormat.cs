using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedReportFragmentsFormat
  {
    string ApplyTo(IReadOnlyList<NamespaceDependencyPath> paths, string header);
  }
}
