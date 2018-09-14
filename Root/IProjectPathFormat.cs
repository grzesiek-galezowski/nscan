using System.Collections.Generic;
using MyTool.App;

namespace MyTool
{
  public interface IProjectPathFormat
  {
    string ApplyTo(List<IReferencedProject> violationPath);
  }
}