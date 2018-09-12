using System.Collections.Generic;
using System.Linq;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class PlainProjectPathFormat : IProjectPathFormat
  {
    public string ApplyTo(List<IReferencedProject> violationPath)
    {
      return violationPath.Skip(1).Aggregate(
        "[" + violationPath.First().ToString() + "]", 
        (total, current) => total + "->" + "[" + current.ToString() + "]");
    }
  }
}