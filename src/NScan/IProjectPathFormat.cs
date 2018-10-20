using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IProjectPathFormat
  {
    string ApplyTo(List<IReferencedProject> violationPath);
  }
}