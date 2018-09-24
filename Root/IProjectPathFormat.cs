using System.Collections.Generic;
using NScanRoot.App;

namespace NScanRoot
{
  public interface IProjectPathFormat
  {
    string ApplyTo(List<IReferencedProject> violationPath);
  }
}