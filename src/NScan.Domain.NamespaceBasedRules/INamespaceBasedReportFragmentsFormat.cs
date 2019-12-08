using System.Collections.Generic;

namespace NScan.Domain.Root
{
  public interface INamespaceBasedReportFragmentsFormat
  {
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}