using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedReportFragmentsFormat
  {
    string ApplyTo(IReadOnlyList<IReadOnlyList<string>> paths, string header);
  }
}