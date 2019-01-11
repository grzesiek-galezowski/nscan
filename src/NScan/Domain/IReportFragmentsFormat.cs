using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IReferencedProject> violationPath);
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}