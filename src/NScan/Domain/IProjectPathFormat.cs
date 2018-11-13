using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IProjectPathFormat
  {
    string ApplyTo(IReadOnlyList<IReferencedProject> violationPath);
  }
}